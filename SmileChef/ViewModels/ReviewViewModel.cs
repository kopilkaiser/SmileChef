using SmileChef.Models.DbModels;

namespace SmileChef.ViewModels
{
    public class ReviewViewModel
    {
        public List<Review> Reviews { get; set; } = new();
        public int RecipeId { get; set; }
    }
}
