using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SmileChef.Extensions;
using SmileChef.Repository;
using SmileChef.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;

namespace ChefApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChefAppContext _context;
        private IHttpContextAccessor _http;
        private IChefRepository _chefRepo;
        private IRepository<Recipe> _recipeRepo;
        private static int? _currentUserId;
        private int _chefId;
        public HomeController(ILogger<HomeController> logger, ChefAppContext context, IChefRepository chefRepo, IHttpContextAccessor httpContextAccessor, IRepository<Recipe> recipeRepo)
        {
            _logger = logger;
            _context = context;
            _chefRepo = chefRepo;
            _recipeRepo = recipeRepo;
            _http = httpContextAccessor;
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
