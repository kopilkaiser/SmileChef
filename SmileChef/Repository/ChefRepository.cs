using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;
using SmileChef.ViewModels;

namespace SmileChef.Repository
{
    public class ChefRepository : GenericRepository<Chef>, IChefRepository
    {
        private ChefAppContext _context;
        public ChefRepository(ChefAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ChefViewModel>> GetChefsWithDetailsAsync()
        {
            var chefs = await _context.Chefs
                                .Include(c => c.User)
                                .Include(c => c.Recipes)
                                    .ThenInclude(r => r.Instructions)
                                .Include(c => c.Recipes)
                                    .ThenInclude(r => r.Reviews)
                                .Include(c => c.SubscribedTo)
                                .Include(c => c.PublishedSubscriptions)
                                .ToListAsync();

            var chefViewModels = chefs.Select(c => new ChefViewModel
            {
                ChefId = c.ChefId,
                ChefName = $"{c.FirstName} {c.LastName}",
                User = c.User,
                Rating = c.Rating,
                SubscriptionCost = c.SubscriptionCost,
                AccountBalance = c.AccountBalance,
                Recipes = c.Recipes.Select(r => new RecipeViewModel
                {
                    RecipeId = r.RecipeId,
                    Name = r.Name,
                    Instructions = r.Instructions.OrderBy(i => i.OrderId).Select(i => new InstructionViewModel
                    {
                        InstructionId = i.InstructionId,
                        Description = i.Description,
                        OrderId = i.OrderId,
                        Duration = i.Duration
                    }).ToList(),
                    Reviews = r.Reviews,
                    RecipeType = r.RecipeType,
                    // Set Price only if it is a PremiumRecipe
                    Price = r is PremiumRecipe premiumRecipe ? premiumRecipe.Price : null
                }).ToList(),
                SubscribedTo = c.SubscribedTo.Select(s => new SubscriptionViewModel
                {
                    SubscriptionId = s.SubscriptionId,
                    SubscriptionDate = s.SubscriptionDate,
                    Amount = s.Amount,
                    SubscriptionType = s.SubscriptionType,
                    PublisherName = $"{s.Publisher.FirstName} {s.Publisher.LastName}"
                }).ToList(),
                PublishedSubscriptions = c.PublishedSubscriptions.Select(s => new SubscriptionViewModel
                {
                    SubscriptionId = s.SubscriptionId,
                    SubscriptionDate = s.SubscriptionDate,
                    Amount = s.Amount,
                    SubscriptionType = s.SubscriptionType,
                    SubscriberName = $"{s.Subscriber.FirstName} {s.Subscriber.LastName}"
                }).ToList()
            }).ToList();

            return chefViewModels;
        }

        public List<ChefViewModel> GetChefsWithDetails()
        {
            // Fetch data from domain models
            var chefs = _context.Chefs
                .Include(c => c.User)
                .Include(c => c.Recipes)
                    .ThenInclude(r => r.Instructions)
                .Include(c => c.Recipes)
                    .ThenInclude(r => r.Reviews)
                .Include(c => c.SubscribedTo)
                .Include(c => c.PublishedSubscriptions)
                .ToList();

            // Transform domain models to view models
            var chefsWithDetails = chefs.Select(c => new ChefViewModel
            {
                ChefId = c.ChefId,
                ChefName = $"{c.FirstName} {c.LastName}",
                User = c.User,
                Rating = c.Rating,
                SubscriptionCost = c.SubscriptionCost,
                AccountBalance = c.AccountBalance,
                Recipes = c.Recipes.Select(r => new RecipeViewModel
                {
                    RecipeId = r.RecipeId,
                    Name = r.Name,
                    Instructions = r.Instructions.OrderBy(i => i.OrderId).Select(i => new InstructionViewModel
                    {
                        InstructionId = i.InstructionId,
                        Description = i.Description,
                        OrderId = i.OrderId,
                        Duration = i.Duration
                    }).ToList(),
                    Reviews = r.Reviews,
                    RecipeType = r.RecipeType,
                    // Set Price only if it is a PremiumRecipe
                    Price = r is PremiumRecipe premiumRecipe ? premiumRecipe.Price : null
                }).ToList(),
                SubscribedTo = c.SubscribedTo.Select(s => new SubscriptionViewModel
                {
                    SubscriptionId = s.SubscriptionId,
                    SubscriptionDate = s.SubscriptionDate,
                    Amount = s.Amount,
                    SubscriptionType = s.SubscriptionType,
                    PublisherName = $"{s.Publisher?.FirstName} {s.Publisher?.LastName}"
                }).ToList(),
                PublishedSubscriptions = c.PublishedSubscriptions.Select(s => new SubscriptionViewModel
                {
                    SubscriptionId = s.SubscriptionId,
                    SubscriptionDate = s.SubscriptionDate,
                    Amount = s.Amount,
                    SubscriptionType = s.SubscriptionType,
                    SubscriberName = $"{s.Subscriber?.FirstName} {s.Subscriber?.LastName}"
                }).ToList()
            }).ToList();

            return chefsWithDetails;
        }

        public override Chef? GetById(int id)
        {
            var chef = _context.Chefs
                .Include(c => c.User)
                .Include(c => c.Recipes)
                .ThenInclude(r => r.Instructions)
                .Include(c => c.SubscribedTo)
                .Include(c => c.PublishedSubscriptions)
                .Include(navigationPropertyPath: c => c.User)
                .FirstOrDefault(c => c.ChefId == id);

            if (chef == null)
            {
                throw new Exception($"Chef not found with id: {id}");
            }

            return chef;
        }

        public override List<Chef>? GetAll()
        {
            var chefs = _context.Chefs
                .Include(c => c.User)
                .ToList();
            return chefs;
        }
    }
}

