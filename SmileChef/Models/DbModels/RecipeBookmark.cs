using ChefApp.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("RecipeBookmarks")]
    public class RecipeBookmark
    {
        [Key]
        public int RecipeBookmarkId { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId {get;set;}

        public Recipe? Recipe { get; set; }

        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }

        public Chef Chef { get; set; } = null!;

        public RecipeBookmark()
        {

        }

        public RecipeBookmark(int recipeId, int chefId)
        {
            RecipeId = recipeId;
            ChefId = chefId;
        }
    }
}
