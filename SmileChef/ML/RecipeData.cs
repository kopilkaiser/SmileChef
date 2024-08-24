using Microsoft.ML.Data;

namespace SmileChef.ML
{
    public class RecipeData
    {
        [LoadColumn(0, 4), ColumnName("Features")]
        public string[]? Features { get; set; }

        [LoadColumn(5)]
        public string? RecipeName { get; set; }
    }
}
