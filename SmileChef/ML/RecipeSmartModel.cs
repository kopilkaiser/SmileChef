using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using SkiaSharp;
using System.Data;
using Tensorflow.Contexts;

namespace SmileChef.ML
{
    public class RecipeSmartModel
    {
        private static readonly string _modelPath = Path.Combine("ML", "RecipeAI", "Model", "RecipeModel.zip");
        private static readonly string _dataPath = Path.Combine("ML", "RecipeAI", "Data", "recipes_labelled.txt");
        private static readonly string _testDataSetPath = Path.Combine("ML", "RecipeAI", "Data", "testSet.bin");

        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;

        public RecipeSmartModel()
        {
            _mlContext = new MLContext();
            if (File.Exists(_modelPath))
            {
                _trainedModel = LoadModel();
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(_dataPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_dataPath));
                }

                if (!Directory.Exists(Path.GetDirectoryName(_modelPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_modelPath));
                }

                var data = _mlContext.Data.LoadFromTextFile<RecipeInput>(_dataPath, hasHeader: false, separatorChar: '\t');
                (_trainedModel, _) = TrainAndSaveModel(data);
            }
        }

        private (ITransformer, IDataView) TrainAndSaveModel(IDataView data)
        {

            var recipeInputs = _mlContext.Data.CreateEnumerable<RecipeInput>(data, false);

            var processedData = recipeInputs.Select(recipe => new RecipeInputProcessed
            {
                RecipeId = recipe.RecipeId,
                Ingredients = recipe.Ingredients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Label = recipe.RecipeName
            });

            var processedDataView = _mlContext.Data.LoadFromEnumerable<RecipeInputProcessed>(processedData);

            // Split the data into training and test sets
            var trainTestSplit = _mlContext.Data.TrainTestSplit(processedDataView, testFraction: 0.2, seed: 123);
            var trainData = trainTestSplit.TrainSet;
            var testData = trainTestSplit.TestSet;

            // Save the test set for later evaluation
            using (var fileStream = new FileStream(_testDataSetPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Data.SaveAsBinary(testData, fileStream);
            }

            // Define the data processing pipeline
            var dataProcessPipeline = _mlContext.Transforms.Text
                .FeaturizeText("IngredientsFeaturized", new TextFeaturizingEstimator.Options
                {
                    WordFeatureExtractor = new WordBagEstimator.Options
                    {
                        NgramLength = 2,
                        UseAllLengths = false,
                    },
                    Norm = TextFeaturizingEstimator.NormFunction.L2,
                    StopWordsRemoverOptions = new StopWordsRemovingEstimator.Options()
                }, nameof(RecipeInputProcessed.Ingredients))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(_mlContext.Transforms.Concatenate("Features", "IngredientsFeaturized"))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"));

            // Choose a different trainer
            var trainer = _mlContext.MulticlassClassification.Trainers.LightGbm(new Microsoft.ML.Trainers.LightGbm.LightGbmMulticlassTrainer.Options
            {
                LabelColumnName = "Label",
                FeatureColumnName = "Features",
                NumberOfIterations = 100,
                LearningRate = 0.1,
                NumberOfLeaves = 31,
                MinimumExampleCountPerLeaf = 20,
                UseCategoricalSplit = true,
                HandleMissingValue = true
            })
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Create an ML pipeline
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // Train the model
            var trainedModel = trainingPipeline.Fit(trainData);

            /* Commented out 28/07/2024 to incorporate new pipeline
            var trainTestSplit = _mlContext.Data.TrainTestSplit(processedDataView, testFraction: 0.2);

            
            // using <SdcaMaximumEntropy> trainer
            var pipeline = _mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "RecipeName", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Ingredients", outputColumnName: "Features"))
                .AppendCacheCheckpoint(_mlContext);

            //create trainingPipeline
            var trainingPipeline = pipeline
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = trainingPipeline.Fit(trainTestSplit.TrainSet);
            */

            _mlContext.Model.Save(trainedModel, trainTestSplit.TrainSet.Schema, _modelPath);

            // Use cross-validation to evaluate the model
            var cvResults = _mlContext.MulticlassClassification.CrossValidate(processedDataView, trainingPipeline, numberOfFolds: 5);

            // Aggregate metrics
            var logLoss = cvResults.Average(r => r.Metrics.LogLoss);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(r => r.Metrics.MacroAccuracy);

            Console.WriteLine($"Average LogLoss: {logLoss}");
            Console.WriteLine($"Average MicroAccuracy: {microAccuracy}");
            Console.WriteLine($"Average MacroAccuracy: {macroAccuracy}");

            return (trainedModel, trainTestSplit.TestSet);
        }

        private ITransformer LoadModel()
        {
            var model = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            // Load the test set

            if (File.Exists(_testDataSetPath))
            {
                IDataView testData = _mlContext.Data.LoadFromBinary(_testDataSetPath);

                // Evaluate the model on the test data
                var predictions = model.Transform(testData);
                var metrics = _mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

                // Log model statistics to the console
                Console.WriteLine($"Log-loss: {metrics.LogLoss}");
                Console.WriteLine($"Per Class Log-loss: {string.Join(", ", metrics.PerClassLogLoss.Select((loss, index) => $"Class {index}: {loss}"))}");
                Console.WriteLine($"Macro accuracy: {metrics.MacroAccuracy}");
                Console.WriteLine($"Micro accuracy: {metrics.MicroAccuracy}");
            }
            else
            {
                Console.WriteLine("Test set not found. Cannot evaluate the model.");
            }


            return model;
        }

        public RecipeOutput Predict(string ingredients)
        {
            // Split ingredients string into an array
            var ingredientsArray = ingredients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RecipeInputProcessed, RecipeOutput>(_trainedModel);
            var prediction = predictionEngine.Predict(new RecipeInputProcessed { Ingredients = ingredientsArray });

            /* old code used with the old SdcaMaximumEntropy trainer
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RecipeInput, RecipeOutput>(_trainedModel);
            var prediction = predictionEngine.Predict(new RecipeInput { Ingredients = ingredients });
            */

            return prediction;
        }
    }
}
