using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using SkiaSharp;
using System.Data;

namespace SmileChef.ML
{
    public class RecipeSmartModel
    {
        private static readonly string _modelPath = Path.Combine("ML", "Model", "RecipeModel.zip");
        private static readonly string _dataPath = Path.Combine("ML", "Data", "recipes_labelled.txt");
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

                var data = _mlContext.Data.LoadFromTextFile<RecipeInput>(_dataPath, hasHeader: true, separatorChar: '\t');
                (_trainedModel, _) = TrainAndSaveModel(data);
            }
        }

        private (ITransformer, IDataView) TrainAndSaveModel(IDataView data)
        {


            var trainTestSplit = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

            // using <SdcaMaximumEntropy> trainer
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "RecipeName", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Ingredients", outputColumnName: "Features"))
                .AppendCacheCheckpoint(_mlContext);

            //create trainingPipeline
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
            .Append(estimator: _mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = trainingPipeline.Fit(trainTestSplit.TrainSet);

            _mlContext.Model.Save(model, trainTestSplit.TrainSet.Schema, _modelPath);

            // Use cross-validation to evaluate the model
            var cvResults = _mlContext.MulticlassClassification.CrossValidate(data, trainingPipeline, numberOfFolds: 5);

            // Aggregate metrics
            var logLoss = cvResults.Average(r => r.Metrics.LogLoss);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(r => r.Metrics.MacroAccuracy);

            Console.WriteLine($"Average LogLoss: {logLoss}");
            Console.WriteLine($"Average MicroAccuracy: {microAccuracy}");
            Console.WriteLine($"Average MacroAccuracy: {macroAccuracy}");

            return (model, trainTestSplit.TestSet);
        }

        private ITransformer LoadModel()
        {
            return _mlContext.Model.Load(_modelPath, out _);
        }

        public RecipeOutput Predict(string ingredients)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RecipeInput, RecipeOutput>(_trainedModel);
            var prediction = predictionEngine.Predict(new RecipeInput { Ingredients = ingredients });
            return prediction;
        }
    }
}
