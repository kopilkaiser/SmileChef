using Microsoft.ML;
using Microsoft.ML.Data;

namespace SmileChef.ML
{
    // <SnippetInceptionSettings>
    struct InceptionSettings
    {
        public const int ImageHeight = 224;
        public const int ImageWidth = 224;
        public const float Mean = 117;
        public const float Scale = 1;
        public const bool ChannelsLast = true;
    }
    // </SnippetInceptionSettings>

    // <SnippetDeclareImageData>
    public class ImageData
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;
    }
    // </SnippetDeclareImageData>

    // <SnippetDeclareImagePrediction>
    public class ImagePrediction : ImageData
    {
        public float[] Score;

        public string PredictedLabelValue;
    }

    public class ImageSmartModel
    {
        // <SnippetDeclareGlobalVariables>
        private string _assetsPath;
        private string _imagesFolder;
        private string _trainTagsTsv;
        private string _testTagsTsv;
        private string _predictSingleImage;
        private string _inceptionTensorFlowModel;
        private MLContext _mlContext;
        private ITransformer _model;
        private string _savedModelPath;
        public ImageSmartModel()
        {
            _assetsPath = Path.Combine(Environment.CurrentDirectory, "ML", "ImageAI", "assets");
            _imagesFolder = Path.Combine(_assetsPath, "imagesNew");
            _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
            _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
            _predictSingleImage = Path.Combine(_imagesFolder, "sphagetti4.jpg");
            _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");
            _savedModelPath = Path.Combine(Environment.CurrentDirectory, "ML", "ImageAI", "Model", "MLModel.zip");

            _mlContext = new MLContext();
            try
            {
                _model = LoadModel(_mlContext);
            }
            catch (FileNotFoundException)
            {
                _model = GenerateModel(_mlContext);
            }
            ClassifySingleImage(_mlContext, _model);

        }
        // Build and train model
        private ITransformer GenerateModel(MLContext mlContext)
        {
            // <SnippetImageTransforms>
            IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: _imagesFolder, inputColumnName: nameof(ImageData.ImagePath))
                            // The image transforms transform the images into the model's expected format.
                            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                            // </SnippetImageTransforms>
                            // The ScoreTensorFlowModel transform scores the TensorFlow model and allows communication
                            // <SnippetScoreTensorFlowModel>
                            .Append(mlContext.Model.LoadTensorFlowModel(_inceptionTensorFlowModel).
                                ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                            // </SnippetScoreTensorFlowModel>
                            // <SnippetMapValueToKey>
                            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
                            // </SnippetMapValueToKey>
                            // <SnippetAddTrainer>
                            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                            // </SnippetAddTrainer>
                            // <SnippetMapKeyToValue>
                            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
                            .AppendCacheCheckpoint(mlContext);
            // </SnippetMapKeyToValue>

            // <SnippetLoadData>
            IDataView trainingData = mlContext.Data.LoadFromTextFile<ImageData>(path: _trainTagsTsv, ',', hasHeader: false);
            // </SnippetLoadData>

            // Train the model
            Console.WriteLine("=============== Training classification model ===============");
            // Create and train the model
            // <SnippetTrainModel>
            ITransformer model = pipeline.Fit(trainingData);

            // Save the trained model
            mlContext.Model.Save(model, trainingData.Schema, _savedModelPath);
            Console.WriteLine($"Model saved to: {_savedModelPath}");
            // </SnippetTrainModel>

            // Generate predictions from the test data, to be evaluated
            // <SnippetLoadAndTransformTestData>
            IDataView testData = mlContext.Data.LoadFromTextFile<ImageData>(path: _testTagsTsv, ',', hasHeader: false);
            IDataView predictions = model.Transform(testData);
            // Create an IEnumerable for the predictions for displaying results
            IEnumerable<ImagePrediction> imagePredictionData = mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
            DisplayResults(imagePredictionData);
            // </SnippetLoadAndTransformTestData>

            // Get performance metrics on the model using training data
            Console.WriteLine("=============== Classification metrics ===============");

            // <SnippetEvaluate>
            MulticlassClassificationMetrics metrics =
                mlContext.MulticlassClassification.Evaluate(predictions,
                  labelColumnName: "LabelKey",
                  predictedLabelColumnName: "PredictedLabel");
            // </SnippetEvaluate>

            //<SnippetDisplayMetrics>
            Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");
            //</SnippetDisplayMetrics>

            // <SnippetReturnModel>
            return model;
            // </SnippetReturnModel>
        }

        //Load the model
        private ITransformer LoadModel(MLContext mlContext)
        {
            if (!File.Exists(_savedModelPath))
            {
                throw new FileNotFoundException($"Model file not found at path: {_savedModelPath}");
            }

            DataViewSchema modelSchema;
            ITransformer model = mlContext.Model.Load(_savedModelPath, out modelSchema);
            Console.WriteLine($"Model loaded from: {_savedModelPath}");

            return model;
        }
        private void ClassifySingleImage(MLContext mlContext, ITransformer model, string imageUrl = "")
        {
            var imageUrlToPredict = string.IsNullOrEmpty(imageUrl) ? _predictSingleImage : imageUrl;
            // load the fully qualified image file name into ImageData
            // <SnippetLoadImageData>
            var imageData = new ImageData()
            {
                ImagePath = imageUrlToPredict
            };
            // </SnippetLoadImageData>

            // <SnippetPredictSingle>
            // Make prediction function (input = ImageData, output = ImagePrediction)
            var predictor = mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            var prediction = predictor.Predict(imageData);
            // </SnippetPredictSingle>

            Console.WriteLine("=============== Making single image classification ===============");
            // <SnippetDisplayPrediction>
            Console.WriteLine($"Image: {Path.GetFileName(imageData.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            // </SnippetDisplayPrediction>
        }

        public string ClassifySingleImage(string imageUrl = "")
        {
            var imageUrlToPredict = string.IsNullOrEmpty(imageUrl) ? _predictSingleImage : Path.Combine(_imagesFolder, imageUrl);
            // load the fully qualified image file name into ImageData
            // <SnippetLoadImageData>
            var imageData = new ImageData()
            {
                ImagePath = imageUrlToPredict
            };
            // </SnippetLoadImageData>

            // <SnippetPredictSingle>
            // Make prediction function (input = ImageData, output = ImagePrediction)
            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(_model);
            var prediction = predictor.Predict(imageData);
            // </SnippetPredictSingle>

            Console.WriteLine("=============== Making single image classification ===============");
            // <SnippetDisplayPrediction>
            Console.WriteLine($"Image: {Path.GetFileName(imageData.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            // </SnippetDisplayPrediction>
            return prediction.PredictedLabelValue;
        }

        void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
        {
            // <SnippetDisplayPredictions>
            foreach (ImagePrediction prediction in imagePredictionData)
            {
                Console.WriteLine($"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            }
            // </SnippetDisplayPredictions>
        }

    }
}
