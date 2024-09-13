using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using SmileChef.Models.DbModels;
using SmileChef.Repository;

namespace SmileChef.Controllers
{
    public class AdminController : Controller
    {
        #region Repositories
        private readonly IChefRepository _chefRepo;
        private readonly IRepository<Instruction> _instructionRepo;
        private readonly IRepository<Recipe> _recipeRepo;
        private readonly IRepository<Subscription> _subscriptionRepo;
        private readonly IRepository<NotifySubscribers> _notifySubscribersRepo;
        private readonly IRepository<Review> _reviewRepo;
        private readonly IRepository<RecipeBookmark> _bookmarkRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<SupportMessage> _supportRepo;
        #endregion

        public AdminController(IChefRepository chefRepo, IRepository<Instruction> instructionRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subscriptionRepo, IRepository<NotifySubscribers> notifySubscribersRepo, IRepository<Review> reviewRepo, IRepository<RecipeBookmark> bookmarkRepo, IRepository<Order> orderRepo, IRepository<SupportMessage> supportRepo)
        {
            _chefRepo = chefRepo;
            _instructionRepo = instructionRepo;
            _recipeRepo = recipeRepo;
            _subscriptionRepo = subscriptionRepo;
            _notifySubscribersRepo = notifySubscribersRepo;
            _reviewRepo = reviewRepo;
            _bookmarkRepo = bookmarkRepo;
            _orderRepo = orderRepo;
            _supportRepo = supportRepo;
        }

        public IActionResult Index()
        {
            var currentUserId = HttpContext?.Session.GetInt32("CurrentChefId");
            var getChefAdmin = _chefRepo.GetAll().FirstOrDefault(c => c.UserId == currentUserId);
            var user = getChefAdmin.User;
            return View(user);
        }

        [HttpGet]
        public IActionResult ShowIssues()
        {
            var issues = _supportRepo.GetAll();
            return View(issues);
        }
    }
}
