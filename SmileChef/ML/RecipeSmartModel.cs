using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using SkiaSharp;
using System.Data;
using Tensorflow.Contexts;
using static TorchSharp.torch.utils;

namespace SmileChef.ML
{
    public class RecipeSmartModel
    {
        private static readonly string _modelPath = Path.Combine("ML", "RecipeAI", "Model", "RecipeModel.zip");
        private static readonly string _dataPath = Path.Combine("ML", "RecipeAI", "Data", "recipes_labelled.txt");

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

                var data = _mlContext.Data.LoadFromTextFile<RecipeData>(_dataPath, hasHeader: true, separatorChar: ';');
                _trainedModel = TrainAndSaveModel(data);
            }
        }

        private ITransformer TrainAndSaveModel(IDataView data)
        {
            // Split the data into training and testing sets (e.g., 80% training, 20% testing)
            //var trainTestSplit = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            //var trainData = trainTestSplit.TrainSet;
            //var testData = trainTestSplit.TestSet;

            // Preprocess the data and set up the pipeline
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText("FeaturesFeaturized", "Features")
                .Append(_mlContext.Transforms.Concatenate("Features", "FeaturesFeaturized"))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", "RecipeName"));

            // Choose the training algorithm
            var trainer = _mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features")
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            // Create and train the model
            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(data);

            // Save the trained model
            _mlContext.Model.Save(trainedModel, data.Schema, _modelPath);

            // Evaluate the model on the test set
            var testMetrics = _mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(data), labelColumnName: "Label", scoreColumnName: "Score");

            // Log various metrics
            Console.WriteLine($"Log-loss: {testMetrics.LogLoss}");
            Console.WriteLine($"Log-loss reduction: {testMetrics.LogLossReduction}");
            Console.WriteLine($"Macro accuracy: {testMetrics.MacroAccuracy}");
            Console.WriteLine($"Micro accuracy: {testMetrics.MicroAccuracy}");
            Console.WriteLine($"Top K accuracy: {testMetrics.TopKAccuracy}");
            Console.WriteLine($"Top K prediction count: {testMetrics.TopKPredictionCount}");

            return trainedModel;
        }

        private ITransformer LoadModel()
        {
            var model = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            // Evaluate the loaded model if necessary
            if (File.Exists(_dataPath))
            {
                var testData = _mlContext.Data.LoadFromTextFile<RecipeData>(_dataPath, separatorChar: ';', hasHeader: true);
                var metrics = _mlContext.MulticlassClassification.Evaluate(model.Transform(testData), labelColumnName: "Label", scoreColumnName: "Score");

                Console.WriteLine($"Log-loss: {metrics.LogLoss}");
                Console.WriteLine($"Macro accuracy: {metrics.MacroAccuracy}");
                Console.WriteLine($"Micro accuracy: {metrics.MicroAccuracy}");
            }

            return model;
        }

        public RecipePrediction Predict(string ingredients)
        {
            // Split ingredients string into an array
            var ingredientsArray = ingredients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RecipeData, RecipePrediction>(_trainedModel);
            var prediction = predictionEngine.Predict(new RecipeData { Features = ingredientsArray });

            return prediction;
        }
    }
}
