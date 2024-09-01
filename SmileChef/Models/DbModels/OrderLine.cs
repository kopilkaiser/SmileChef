using ChefApp.Models.DbModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("OrderLines")]
    public class OrderLine
    {
        public int OrderLineId { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public string? RecipeName { get; set; }
        public Recipe? Recipe { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
