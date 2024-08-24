using Microsoft.ML.Data;

namespace SmileChef.ML
{
    public class RecipePrediction
    {
        [ColumnName("PredictedLabel")]
        public string? PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float[]? Score { get; set; }
    }
}
