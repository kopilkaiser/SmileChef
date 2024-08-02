using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmileChef.Extensions;
using SmileChef.ML;
using SmileChef.Models;
using SmileChef.Repository;
using SmileChef.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;

namespace ChefApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<HomeController> _logger;
        private readonly ChefAppContext _context;
        private IHttpContextAccessor _http;
        private IChefRepository _chefRepo;
        private IRepository<Recipe> _recipeRepo;
        private static int? _currentUserId;
        private int _chefId;
        private readonly RecipeSmartModel _recipeModel;
        private readonly ImageSmartModel _imageModel;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, ChefAppContext context, IChefRepository chefRepo, IHttpContextAccessor httpContextAccessor, IRepository<Recipe> recipeRepo, RecipeSmartModel recipeModel, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _chefRepo = chefRepo;
            _recipeRepo = recipeRepo;
            _http = httpContextAccessor;
            _recipeModel = recipeModel;
            _imageModel = new ImageSmartModel();
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index(bool json = false)
        {

            ViewBag.CurrentActive = "Home";

            _currentUserId = _http.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");

            if (_currentUserId == 0)
            {
                _currentUserId = _config.GetValue<int>("CurrentChefId"); // Default user ID if not set
                HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
            }

            return View("HomeIndex");
        }

 
        public IActionResult Privacy()
        {
            ViewBag.CurrentActive = "Privacy";
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UnderstandingCustomValidation(ChefViewModel model)
        {

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);

            // Validate only specific properties
            var propertiesToValidate = new[] { nameof(ChefViewModel.ChefName), nameof(ChefViewModel.User.Email), nameof(ChefViewModel.User.Password) };

            foreach (var property in propertiesToValidate)
            {
                var propertyInfo = typeof(ChefViewModel).GetProperty(property);
                var value = propertyInfo.GetValue(model);
                var results = new List<ValidationResult>();
                var validationContext = new ValidationContext(model) { MemberName = property };

                if (!Validator.TryValidateProperty(value, validationContext, results))
                {
                    validationResults.AddRange(results);
                }
            }

            if (validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                }
                return View(model);
            }

            // Proceed with valid model
            return RedirectToAction("Success");
        }

        private void AssignChefId()
        {
            var getChef = _chefRepo.GetAll().FirstOrDefault(c => c.UserId == _currentUserId);
            if (getChef == null)
            {
                throw new Exception($"Cannot find any chef with userId: {_currentUserId}");
            }

            _chefId = getChef.ChefId;
        }
    }
}
