using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmileChef.Extensions;
using SmileChef.ML;
using SmileChef.Models;
using SmileChef.Repository;
using SmileChef.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static System.Net.WebRequestMethods;

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
        private static int? _currentUserId;
        private int _chefId;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RecipeSmartModel _recipeModel;

        public ChefController(ILogger<ChefController> logger, IHttpContextAccessor httpContextAccessor, IChefRepository chefRepo, IRepository<Instruction> instructRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subRepo, IWebHostEnvironment webHostEnvironment, RecipeSmartModel recipeModel)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _chefRepo = chefRepo;
            _instructRepo = instructRepo;
            _recipeRepo = recipeRepo;
            _subRepo = subRepo;
            _webHostEnvironment = webHostEnvironment;
            _recipeModel = recipeModel;
        }

        //[HttpGet("X")]
        //public IActionResult Index()
        //{
        //    ViewBag.CurrentActive = "ChefIndex";
        //    _currentUserId = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");

        //    if (_currentUserId == 0)
        //    {
        //        _currentUserId = 1; // Default user ID if not set
        //        HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
        //    }
        //    return View();
        //}

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

        #region Recipe Management

        [HttpGet("/")]
        [HttpGet("Index")]
        public IActionResult ManageRecipes(bool json = false)
        {
            ViewBag.CurrentActive = "ManageRecipes";
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");

            _currentUserId = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");

            if (_currentUserId == 0)
            {
                _currentUserId = 2; // Default user ID if not set
                HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
            }

            var chefVM = _chefRepo.GetChefsWithDetails().Find(c => c.User.UserId == _currentUserId);

            if (HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail") == null)
            {
                HttpContext.Session.SetObjectAsJson("CurrentUserEmail", chefVM.User.Email);
                HttpContext.Session.SetObjectAsJson("CurrentUserName", chefVM.ChefName);
            }

            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            if (chefVM == null)
            {
                // Handle the case where the chef is not found, perhaps redirect to an error page or return a default view.
                return RedirectToAction("Error", "Home");
            }

            if (json == true) return Json(chefVM);

            var recipeMessage = ViewBag.AddRecipeMessage;
            var recipeIsSuccess = ViewBag.RecipeSuccess;
            var tempRecipeMessage = TempData["RecipeSuccessMessage"];
            var tempRecipeSuccess = TempData["RecipeSuccess"];
            return View(chefVM);
        }

        [HttpGet]
        public IActionResult AddRecipe(int recipeId)
        {
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");

            RecipeViewModel vm;
            if (recipeId == 0)
            {
                vm = new RecipeViewModel();
                vm.Instructions = new();
            }
            else
            {
                AssignChefId();
                var chef = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.ChefId == _chefId);
                vm = chef.Recipes.FirstOrDefault(r => r.RecipeId == recipeId)!;
            }
            return View("AddOrUpdateRecipe", vm);
        }

        [HttpPost]
        public IActionResult SaveRecipe(RecipeViewModel model)
        {
            // Remove entries for removed instructions from the ModelState
            var keysToRemove = ModelState.Keys
                .Where(key => key.StartsWith("Instructions") && model.Instructions.Any(i => i.IsRemoved && key.Contains($"Instructions[{model.Instructions.IndexOf(i)}]")))
                .ToList();

            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (ModelState.IsValid)
            {
                AssignChefId();
                var chef = _chefRepo.GetById(_chefId);

                if (chef == null) throw new Exception($"Chef not found with ChefId: {_chefId}");

                if (model.Instructions == null) model.Instructions = new List<InstructionViewModel>();
                // Sort instructions by OrderId
                if (model.Instructions != null)
                {
                    // Filter out removed instructions
                    model.Instructions = model.Instructions.Where(i => !i.IsRemoved).ToList();
                    model.Instructions = model.Instructions.OrderBy(i => i.OrderId).ToList();
                }

                Recipe? recipe;
                if (model.RecipeId == 0) // Add new recipe
                {
                    recipe = new Recipe();
                    chef.Recipes.Add(recipe);
                }
                else // Update existing recipe
                {
                    recipe = chef.Recipes.FirstOrDefault(r => r.RecipeId == model.RecipeId);
                    if (recipe == null)
                    {
                        TempData["RecipeSuccessMessage"] = "Recipe not found.";
                        TempData["RecipeSuccess"] = false;
                        return RedirectToAction("Index");
                    }

                    // Remove instructions that are no longer present in the model
                    var instructionsToRemove = recipe.Instructions
                        .Where(ri => !model.Instructions.Any(mi => mi.InstructionId == ri.InstructionId))
                        .ToList();

                    foreach (var instruction in instructionsToRemove)
                    {
                        recipe.Instructions.Remove(instruction);
                    }
                }

                recipe.Name = model.Name;

                foreach (var i in model.Instructions)
                {
                    var instruction = recipe.Instructions.FirstOrDefault(instr => instr.InstructionId == i.InstructionId);
                    if (instruction == null) // Add new instruction
                    {
                        instruction = new Instruction
                        {
                            Description = i.Description,
                            OrderId = i.OrderId,
                        };

                        if (i.Duration != null) instruction.Duration = new TimeSpan(0, i.Duration.Value.Days, 0);

                        recipe.Instructions.Add(instruction);
                    }
                    else // Update existing instruction
                    {
                        instruction.Description = i.Description;
                        instruction.OrderId = i.OrderId;
                        if (i.Duration != null) instruction.Duration = new TimeSpan(0, i.Duration.Value.Days, 0);
                    }
                }

                _chefRepo.Update(chef);

                TempData["RecipeSuccessMessage"] = model.RecipeId == 0 ? "Recipe added successfully" : "Recipe updated successfully";
                TempData["RecipeSuccess"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["RecipeSuccessMessage"] = "Recipe failed to save due to errors";
                TempData["RecipeSuccess"] = false;
                return View("AddOrUpdateRecipe", model); // Pass the original model back to the view
            }
        }

        public IActionResult DeleteRecipe(int id)
        {
            AssignChefId();
            var chef = _chefRepo.GetById(_chefId);
            var recipeToRemove = chef.Recipes.FirstOrDefault(r => r.RecipeId == id);
            if (recipeToRemove == null) { throw new Exception($"Couldn't find recipe with recipeId: {id}"); }

            chef.Recipes.Remove(recipeToRemove);
            _chefRepo.Update(chef);

            TempData["RecipeSuccessMessage"] = "Recipe deleted successfully";
            TempData["RecipeSuccess"] = false;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddInstruction(int recipeId, string recipeName, InstructionViewModel instruction)
        {
            AssignChefId();
            var chef = _chefRepo.GetById(_chefId);
            if (chef == null) throw new Exception($"Chef not found with chefId: {_chefId}");
            Recipe recipe;

            if (recipeId == 0)
            {
                // Create a new recipe if recipeId is 0
                recipe = new Recipe
                {
                    Name = recipeName,
                    Instructions = new List<Instruction>()
                };
                chef.Recipes.Add(recipe);
                _chefRepo.Update(chef);
            }
            else
            {
                // Find the existing recipe
                recipe = chef.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
                if (recipe == null)
                {
                    return NotFound("Recipe not found.");
                }
            }

            // Add the new instruction
            var newInstruction = new Instruction
            {
                Description = instruction.Description ?? "No Description Given",
                OrderId = recipe.Instructions.Count + 1,
            };
            if (instruction.Duration != null)
            {
                newInstruction.Duration = new TimeSpan(0, instruction.Duration.Value.Days, 0);
            }

            recipe.Instructions.Add(newInstruction);
            _chefRepo.Update(chef);

            return RedirectToAction("AddRecipe", new { recipeId = recipe.RecipeId });
        }

        [HttpPost]
        public IActionResult UpdateInstruction(int recipeId, InstructionViewModel instruction)
        {
            AssignChefId();
            var chef = _chefRepo.GetById(_chefId);
            var recipe = chef.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            var existingInstruction = recipe.Instructions.FirstOrDefault(i => i.InstructionId == instruction.InstructionId);
            if (existingInstruction == null)
            {
                return NotFound("Instruction not found.");
            }

            existingInstruction.Description = instruction.Description;
            existingInstruction.OrderId = instruction.OrderId;
            if (instruction.Duration != null) existingInstruction.Duration = new TimeSpan(0, instruction.Duration.Value.Days, 0);

            _chefRepo.Update(chef);

            return RedirectToAction("AddRecipe", new { recipeId = recipeId });
        }

        public IActionResult DeleteInstruction(int recipeId, int instructionId)
        {
            AssignChefId();
            var chef = _chefRepo.GetById(_chefId);
            var recipe = chef.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            var instruction = recipe.Instructions.FirstOrDefault(i => i.InstructionId == instructionId);
            if (instruction == null)
            {
                return NotFound("Instruction not found.");
            }

            recipe.Instructions.Remove(instruction);
            _chefRepo.Update(chef);

            return RedirectToAction("AddRecipe", new { recipeId = recipeId });
        }

        [HttpPost]
        //public IActionResult CustomAjaxTest(string message)
        public IActionResult FilterRecipes([FromBody] JsonElement data)
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
        #endregion

        #region RECIPE & IMAGE AI

        #region RECIPE AI
        [HttpGet]
        public IActionResult RecipeSmartAI()
        {
            ViewBag.CurrentActive = "RecipeSmartAI";
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            var model = new PredictRecipeViewModel();
            //Thread.Sleep(millisecondsTimeout: 3000);
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
        #endregion

        #region IMAGE AI      
        [HttpGet]
        public IActionResult ImageSmartAI()
        {
            ViewBag.CurrentActive = "ImageSmartAI";

            _currentUserId = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");

            if (_currentUserId == 0)
            {
                _currentUserId = 1; // Default user ID if not set
                HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
            }

            return View();
        }

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
        #endregion

        #endregion

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
