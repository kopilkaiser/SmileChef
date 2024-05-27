using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace SmileChef.ViewModels
{
    public class ChefViewModel
    {
        public int ChefId { get; set; }

        [Required(ErrorMessage = "Chef name is required")]
        public string? ChefName { get; set; }
        public List<RecipeViewModel>? Recipes { get; set; }
        public List<SubscriptionViewModel>? SubscribedTo { get; set; }
        public List<SubscriptionViewModel>? PublishedSubscriptions { get; set; }


        public User User { get; set; }
    }
}
