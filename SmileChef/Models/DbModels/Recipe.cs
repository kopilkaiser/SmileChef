using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Recipes")]
    public class Recipe
    {
        public int RecipeId { get; set; }

        public string Name { get; set; } = null!;

        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }

        public Chef Chef { get; set; }

        public List<Instruction> Instructions { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();
    }
}
