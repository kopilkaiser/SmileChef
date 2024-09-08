using ChefApp.Models.DbModels;

namespace SmileChef.ViewModels
{
    public class ChefSubsciptionViewModel
    {
        public List<Chef> Chefs { get; set; } = new();

        public Chef? CurrentChef { get; set; }
    }
}
