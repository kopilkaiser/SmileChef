using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace SmileChef.ViewModels
{
    public class ChefViewModel
    {
        public int ChefId { get; set; }

        [Required(ErrorMessage = "Chef name is required")]
        public string? ChefName { get; set; }
        public List<RecipeViewModel>? Recipes { get; set; } = new();
        public List<SubscriptionViewModel>? SubscribedTo { get; set; } = new();
        public List<SubscriptionViewModel>? PublishedSubscriptions { get; set; } = new();
        public User User { get; set; }
        public int? Rating { get; set; }
        public decimal? SubscriptionCost { get; set; }
        public decimal? AccountBalance { get; set; }
        public List<NotifySubscribers> Notifications { get; set; }
        
        public Restaurant Restaurant { get; set; }
    }
}
