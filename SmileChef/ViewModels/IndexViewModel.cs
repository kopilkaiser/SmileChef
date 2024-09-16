using ChefApp.Models.DbModels;

namespace SmileChef.ViewModels
{
    public class IndexViewModel
    {
        public Chef CurrentChef { get; set; }
        public List<Chef> TopFiveChefs { get; set; } = new List<Chef>();
        public List<Recipe> TopFiveRecipes { get; set; } = new List<Recipe>();
    }
}
