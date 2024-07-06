using ChefApp.Models.DbModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("NotifySubscribers")]
    public class NotifySubscribers
    {
        public int Id { get; set; }
        public int PublisherId { get; set; }

        [ForeignKey("PublisherId")] // FK to Chef (Publisher)
        public Chef? Publisher { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey("RecipeId")] // FK to Recipe
        public Recipe? Recipe { get; set; }

        public int SubscriberId { get; set; }

        [ForeignKey("SubscriberId")] // FK to Chef (Subscriber)
        public Chef? Subscriber { get; set; }  // Renamed to clarify the role
        public bool Notified { get; set; }

        public DateTime PublishedDate { get; set; }
    }
}
