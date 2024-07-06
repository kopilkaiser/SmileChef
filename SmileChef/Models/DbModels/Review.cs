using ChefApp.Models.DbModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("Reviews")]
    public class Review
    {
        public int Id { get; set; }
        public string? Message { get; set; }

        public int ReviewerId { get; set; }

        [ForeignKey(nameof(ReviewerId))]
        public Chef? Reviewer { get; set; }

        public int RecipeId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public Recipe? Recipe { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
