using Microsoft.ML.Data;

namespace SmileChef.ML
{
    public class RecipeInput
    {
        [LoadColumn(0)]
        [ColumnName("RecipeId")]
        public int RecipeId { get; set; }

        [LoadColumn(1)]
        [ColumnName("Ingredients")]
        public string? Ingredients { get; set; }

        [LoadColumn(2)]
        [ColumnName("RecipeName")]
        public string? RecipeName { get; set; }
    }
}
