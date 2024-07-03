using Microsoft.ML.Data;

namespace SmileChef.ML
{
    public class RecipeOutput
    {
        [ColumnName(@"PredictedLabel")]
        public string PredictedLabel { get; set; }
    }
}
