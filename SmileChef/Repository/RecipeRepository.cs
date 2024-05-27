using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace SmileChef.Repository
{
    public class RecipeRepository : GenericRepository<Recipe>
    {
        public RecipeRepository(ChefAppContext context) : base(context)
        {
        }

        public override Recipe? GetById(int id)
        {
            var recipe = _context.Recipes
                .Include(r => r.Instructions)
                .FirstOrDefault(r => r.RecipeId == id);

            if (recipe == null) throw new Exception($"Recipe not found with id: {id}");

            return recipe;
        }
    }
}
