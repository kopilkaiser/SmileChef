using ChefApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChefApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChefAppContext _context;

        public HomeController(ILogger<HomeController> logger, ChefAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var chefsWithDetails = _context.Chefs
                .Include(c => c.Recipes)
                    .ThenInclude(r => r.Instructions)
                .Include(c => c.SubscribedTo)
                .Include(c => c.PublishedSubscriptions)
                .Select(c => new
                {
                    c.ChefId,
                    ChefName = $"{c.FirstName} {c.LastName}",
                    Recipes = c.Recipes.Select(r => new
                    {
                        r.RecipeId,
                        r.Name,
                        Instructions = r.Instructions.OrderBy(i => i.OrderId).Select(i => new
                        {
                            i.Description,
                            i.OrderId
                        })
                    }),
                    SubscribedTo = c.SubscribedTo.Select(s => new
                    {
                        s.SubscriptionId,
                        s.SubscriptionDate,
                        s.Amount,
                        s.SubscriptionType,
                        PublisherName = $"{s.Publisher!.FirstName} {s.Publisher.LastName}"
                    }),
                    PublishedSubscriptions = c.PublishedSubscriptions.Select(s => new
                    {
                        s.SubscriptionId,
                        s.SubscriptionDate,
                        s.Amount,
                        s.SubscriptionType,
                        SubscriberName = $"{s.Subscriber!.FirstName} {s.Subscriber.LastName}"
                    })
                })
                .ToList();

            foreach (var chef in chefsWithDetails)
            {
                Console.WriteLine($"Chef ID: {chef.ChefId}, Name: {chef.ChefName}");
                foreach (var recipe in chef.Recipes)
                {
                    Console.WriteLine($"  Recipe ID: {recipe.RecipeId}, Name: {recipe.Name}");
                    foreach (var instruction in recipe.Instructions)
                    {
                        Console.WriteLine($"    Order ID: {instruction.OrderId}, Description: {instruction.Description}");
                    }
                }
                foreach (var subscription in chef.SubscribedTo)
                {
                    Console.WriteLine($"  Subscribed To: Subscription ID: {subscription.SubscriptionId}, Date: {subscription.SubscriptionDate}, Amount: {subscription.Amount}, Type: {subscription.SubscriptionType}, Publisher: {subscription.PublisherName}");
                }
                foreach (var subscription in chef.PublishedSubscriptions)
                {
                    Console.WriteLine($"  Published Subscription: Subscription ID: {subscription.SubscriptionId}, Date: {subscription.SubscriptionDate}, Amount: {subscription.Amount}, Type: {subscription.SubscriptionType}, Subscriber: {subscription.SubscriberName}");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
