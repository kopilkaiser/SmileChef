using ChefApp.Models.DbModels;
using System.ComponentModel;
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

        [DisplayName("Recipe Name")]
        public string? RecipeName { get; set; }
        public Recipe? Recipe { get; set; }
        public int Quantity { get; set; }

        [DisplayName("Total")]
        public decimal Price { get; set; }
    }
}
