using SmileChef.Models.DbModels;
using SmileChef.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Recipes")]
    public class Recipe
    {
        //Basic Recipe
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "Recipe name is required")]
        [MaxLength(50, ErrorMessage = "Recipe name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }

        public Chef Chef { get; set; }

        public List<Instruction> Instructions { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();

        public RecipeType RecipeType { get; set; }

        public string? ImageUrl { get; set; }
    }
}
