using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using SmileChef.Extensions;
using SmileChef.Repository;
using SmileChef.ViewModels;

namespace SmileChef.Controllers
{
    public class ChefController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChefController> _logger;
        private readonly IChefRepository _chefRepo;
        private IRepository<Instruction> _instructRepo;
        private IRepository<Recipe> _recipeRepo;
        private IRepository<Subscription> _subRepo;

        public ChefController(ILogger<ChefController> logger, IHttpContextAccessor httpContextAccessor, IChefRepository chefRepo, IRepository<Instruction> instructRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subRepo)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _chefRepo = chefRepo;
            _instructRepo = instructRepo;
            _recipeRepo = recipeRepo;
            _subRepo = subRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CurrentActive = "ChefIndex";
            return View();
        }

        [HttpGet]
        public IActionResult ShowChefCards()
        {
            ViewBag.CurrentActive = "ShowChefCards";
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            int currentChefUserId = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                currentChefUserId = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<int>("CurrentUserId");
            }
            var chefs = _chefRepo.GetChefsWithDetails().Where(c => c.User.UserId != currentChefUserId).OrderByDescending(c => c.Rating).ToList();
            return View(chefs);
        }
    }

}
