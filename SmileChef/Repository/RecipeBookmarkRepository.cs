using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class RecipeBookmarkRepository : GenericRepository<RecipeBookmark>
    {
        public RecipeBookmarkRepository(ChefAppContext context) : base(context)
        {
        }

        public override List<RecipeBookmark>? GetAll()
        {
            var recipeBookmarks = _context.RecipeBookmarks
                .Include(b => b.Chef)
                .Include(b => b.Recipe).ToList();
            return recipeBookmarks;
        }
    }
}
