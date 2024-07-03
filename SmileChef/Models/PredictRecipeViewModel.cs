using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SmileChef.Models
{
    public class PredictRecipeViewModel
    {
        [Required]
        public string Ingredients { get; set; }

        [BindNever]
        public string PredictedRecipe { get; set; }
    }
}
