using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace SmileChef.ViewModels
{
    public class RecipeViewModel
    {
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "Recipe name is required")]
        [MaxLength(100, ErrorMessage = "Recipe name cannot exceed 50 characters")]
        public string? Name { get; set; }
        public List<InstructionViewModel> Instructions { get; set; } = new List<InstructionViewModel>();

        public List<Review> Reviews { get; set; } = new();
    }
}
