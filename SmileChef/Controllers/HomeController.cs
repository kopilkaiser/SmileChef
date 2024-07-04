using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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

        public HomeController(ILogger<HomeController> logger, ChefAppContext context, IChefRepository chefRepo, IHttpContextAccessor httpContextAccessor, IRepository<Recipe> recipeRepo, RecipeSmartModel recipeModel, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _chefRepo = chefRepo;
            _recipeRepo = recipeRepo;
            _http = httpContextAccessor;
            _recipeModel = recipeModel;
            _imageModel = new ImageSmartModel();
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(bool json = false)
        {

            ViewBag.CurrentActive = "Home";

            _currentUserId = _http.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");

            if (_currentUserId == 0)
            {
                _currentUserId = 1; // Default user ID if not set
                HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
            }

            return View();
        }

        #region Test Methods to be DELETED

        [HttpGet]
        public IActionResult LoadAnimals()
        {
            List<string> animals = new List<string>()
            {
                "Tiger", "Lion", "Cheetah", "Spider", "Elephant", "Giraffe"
            };

            return Json(animals);
        }

        [HttpGet]
        public IActionResult LoadHumans()
        {
            List<string> humans = new List<string>()
            {
                "John", "Perry", "Charson", "Norsan", "Melissa", "Natasha", "Samantha"
            };

            return Json(humans);
        }

        [HttpGet]
        public IActionResult LoadAliens()
        {

            List<string> aliens = new List<string>()
            {
                "Yoga TX100", "Alien T-Rex 09", "OctaCore Specimen001"
            };

            return Json(aliens);
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file selected";
                return View("Index");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            ViewBag.Message = "File uploaded successfully!";
            var fileUrl = Path.Combine("/uploads", file.FileName);
            // Get the list of uploaded image URLs from the session
            List<string> uploadedImages = HttpContext.Session.GetObjectFromJson<List<string>>("UploadedImages") ?? new List<string>();
            uploadedImages.Add(fileUrl);

            // Store the updated list back in the session
            HttpContext.Session.SetObjectAsJson("UploadedImages", uploadedImages);

            return Json(new { success = true, fileUrl, uploadedImages });
            //return View("Index");
        }

        [HttpGet]
        public IActionResult GetUploadedImages()
        {
            List<string> uploadedImages = HttpContext.Session.GetObjectFromJson<List<string>>("UploadedImages") ?? new List<string>();
            return Json(new { success = true, uploadedImages });
        }

        [HttpGet]
        public IActionResult RecipeSmartAI()
        {
            ViewBag.CurrentActive = "RecipeSmartAI";
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            var model = new PredictRecipeViewModel();
            Thread.Sleep(millisecondsTimeout: 3000);
            return View(model);
        }

        [HttpPost]
        public IActionResult Predict(PredictRecipeViewModel model)
        {
            // Manually remove the PredictedRecipe property from the ModelState
            ModelState.Remove(nameof(PredictRecipeViewModel.PredictedRecipe));

            if (!ModelState.IsValid)
            {
                // Return JSON response with error message
                return Json(new { success = false, message = "Please enter valid ingredients." });
            }

            var prediction = _recipeModel.Predict(model.Ingredients);
            model.PredictedRecipe = prediction.PredictedLabel;

            // Return the partial view with the prediction result
            return PartialView("_PredictionResultPartial", model);
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

        [HttpPost]
        //public IActionResult CustomAjaxTest(string message)
        public IActionResult CustomAjaxTest([FromBody] JsonElement data)
        {
            var message = data.GetProperty("filterString").GetString();

            AssignChefId();
            var chefVM = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.ChefId == _chefId);

            if (chefVM == null)
            {
                return Json("Error");
            }

            chefVM.Recipes = chefVM.Recipes.Where(r => r.Name.Contains(message, StringComparison.OrdinalIgnoreCase)).ToList();

            if (chefVM.Recipes.Count == 0)
            {
                return Json("Error");
            }

            return PartialView("_RecipesPartial", chefVM);
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
