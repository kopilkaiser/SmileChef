using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SmileChef.Models
{
    public class PredictRecipeViewModel
    {
        [Required]
        public string Ingredients { get; set; }

        [ValidateNever]
        [BindNever]
        public string PredictedRecipe { get; set; }

        [ValidateNever]
        public List<Recipe> SuggestedRecipes { get; set; }
    }
}
