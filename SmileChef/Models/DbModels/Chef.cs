using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Chefs")]
    [PrimaryKey(nameof(ChefId))]
    public class Chef
    {
        public int ChefId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        [InverseProperty(nameof(Subscription.Subscriber))]
        public List<Subscription> SubscribedTo { get; set; } = new();

        [InverseProperty(nameof(Subscription.Publisher))]
        public List<Subscription> PublishedSubscriptions { get; set; } = new();

        public List<Recipe> Recipes { get; set; } = new();

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [MinLength(0)]
        [MaxLength(5)]
        public int? Rating { get; set; }

        public decimal? SubscriptionCost { get; set; }

        public decimal? AccountBalance { get; set; }

        public List<RecipeBookmark> RecipeBookmarks { get; set; } = new();

        public List<Order> Orders { get; set; }
    }
}
