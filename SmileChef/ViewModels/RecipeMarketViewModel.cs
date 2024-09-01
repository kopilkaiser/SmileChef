using ChefApp.Models.DbModels;
using SmileChef.Models.DbModels;

namespace SmileChef.ViewModels
{
    public class RecipeMarketViewModel
    {
        public List<Recipe> Recipes { get; set; }
        public List<RecipeBookmark> CurrentBookmarks { get; set; }
    }
}
