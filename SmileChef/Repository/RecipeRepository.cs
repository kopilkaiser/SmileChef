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
                .Include(recipe => recipe.Instructions)
                .Include(recipe => recipe.Reviews)
                .ThenInclude(review => review.Reviewer)
                .Include(recipe => recipe.Chef)
                .FirstOrDefault(recipe => recipe.RecipeId == id);

            if (recipe == null) throw new Exception($"Recipe not found with id: {id}");

            return recipe;
        }

        public override List<Recipe> GetAll()
        {
            var recipes = _context.Recipes
                .Include(r => r.Instructions)
                .Include(r => r.Reviews)
                .ThenInclude(review => review.Reviewer)
                .Include(r => r.Chef).ToList();

            return recipes;
        }
    }
}
