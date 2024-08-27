using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SmileChef.Extensions;
using SmileChef.ML;
using SmileChef.Models;
using SmileChef.Models.DbModels;
using SmileChef.Repository;
using SmileChef.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using System.IO;
using SmileChef.Models.Enums;
using System.Linq;

namespace SmileChef.Controllers
{
    public class ChefController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
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
        private readonly ImageSmartModel _imageModel;
        private IRepository<NotifySubscribers> _notifySubscribers;
        private IRepository<Review> _reviewRepo;
        private readonly IConfiguration _config;
        public ChefController(ILogger<ChefController> logger, IHttpContextAccessor httpContextAccessor, IChefRepository chefRepo, IRepository<Instruction> instructRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subRepo, IWebHostEnvironment webHostEnvironment, RecipeSmartModel recipeModel, IRepository<NotifySubscribers> notifySubscribers, IRepository<Review> reviewRepo, IConfiguration config, ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _chefRepo = chefRepo;
            _instructRepo = instructRepo;
            _recipeRepo = recipeRepo;
            _subRepo = subRepo;
            _webHostEnvironment = webHostEnvironment;
            _recipeModel = recipeModel;
            _imageModel = new ImageSmartModel();
            _notifySubscribers = notifySubscribers;
            _reviewRepo = reviewRepo;
            _config = config;
            _viewEngine = viewEngine;
        }

        public IActionResult Index()
        {
            ViewBag.CurrentActive = "ChefIndex";
            _currentUserId = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<int>("CurrentUser");
            if (_currentUserId == 0)
            {
                _currentUserId = _config.GetValue<int>("CurrentChefId"); // Default user ID if not set
                HttpContext.Session.SetObjectAsJson("CurrentUserId", _currentUserId);
            }

            var chefVM = _chefRepo.GetChefsWithDetails().Find(c => c.User.UserId == _currentUserId);

            if (HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail") == null)
            {
                HttpContext.Session.SetObjectAsJson("CurrentUserEmail", chefVM.User.Email);
                HttpContext.Session.SetObjectAsJson("CurrentUserName", chefVM.ChefName);
                HttpContext.Session.SetObjectAsJson("CurrentUserBalance", chefVM.AccountBalance);

            }

            AssignCurrentPageStatus("");

            var viewModel = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.User.UserId == _currentUserId);
            var userNotifications = _notifySubscribers.GetAll().Where(n => n.SubscriberId == viewModel.ChefId);

            if (userNotifications.Count() > 0) viewModel.Notifications = userNotifications.ToList();
            else viewModel.Notifications = new List<NotifySubscribers>();

            return View("ChefIndex", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowChefCards(bool showSubscriptionModal = false, bool showUnsubscribedMessage = false)
        {
            AssignChefId();
            AssignCurrentPageStatus("ShowChefCards");
            int currentChefUserId = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                currentChefUserId = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<int>("CurrentUserId");
            }
            var chefs = (await _chefRepo.GetChefsWithDetailsAsync()).Where(c => c.User.UserId != currentChefUserId).OrderByDescending(c => c.Rating).ToList();
            var currentChef = GetCurrentChef();

            ChefSubsciptionViewModel vm = new();
            vm.Chefs = chefs;
            vm.CurrentChef = currentChef;

            if (showSubscriptionModal)
            {
                @ViewBag.ShowSubscriptionModal = true;
            }
            else @ViewBag.ShowSubscriptionModal = false;

            ViewBag.ShowUnsubscribedMessage = showUnsubscribedMessage;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ManageSubscription(int chefId, string returnUrl)
        {
            bool showUnsubscribedMessage = false;

            var currentChef = GetCurrentChef();
            var destChef = (await _chefRepo.GetChefsWithDetailsAsync()).FirstOrDefault(c => c.ChefId == chefId);

            var subscription = currentChef.SubscribedTo.FirstOrDefault(s => s.PublisherName == destChef.ChefName);
            HttpContext.Session.SetObjectAsJson(key: "ChefIdToSubscribe", destChef.ChefId);

            if (subscription == null)
            {
                // means currentChef will subscribe to destChef (Subscript to subscription)
                return RedirectToAction(returnUrl, new { showSubscriptionModal = true });
            }
            else
            {
                // means currentChef will unsubscribe to destChef (Unscribe to subscription)
                var sub = _subRepo.GetById(subscription.SubscriptionId);
                _subRepo.Delete(sub);
                showUnsubscribedMessage = true;
            }

           return RedirectToAction(returnUrl, new { showUnsubscribedMessage });       
        }

        [HttpPost]
        public async Task<IActionResult> ProcessSubscription([FromBody] JsonElement query)
        {
            //create subscription
            var destChefId = HttpContext.Session.GetObjectFromJson<int>("ChefIdToSubscribe");
            var subscription = new Subscription();
            subscription.PublisherId = destChefId;
            subscription.SubscriberId = GetCurrentChef().ChefId;
            subscription.SubscriptionDate = DateTime.Now;
            subscription.Amount = Convert.ToDecimal(query.GetProperty("subAmount").GetString());
            if (Enum.TryParse<SubscriptionType>(query.GetProperty("subType").GetString(), out var subType)){
                subscription.SubscriptionType = subType;
            }

            _subRepo.Add(subscription);
            return Json(new { success = true});
        }

        [HttpGet]
        public async Task<IActionResult> ViewChefDetails(int chefId, bool showSubscriptionModal = false, bool showUnsubscribedMessage = false)
        {
            AssignCurrentPageStatus("");

            if (chefId == 0) chefId = HttpContext.Session.GetObjectFromJson<int>("ChefIdToSubscribe");
            var chefViewModel = (await _chefRepo.GetChefsWithDetailsAsync()).FirstOrDefault(c => c.ChefId == chefId);
            var loggedChef = GetCurrentChef();

            bool isSubscribed = loggedChef.SubscribedTo.FirstOrDefault(s => s.PublisherName == chefViewModel.ChefName) != null;

            ViewBag.IsSubscribed = isSubscribed;

            ViewBag.ShowSubscriptionModal = showSubscriptionModal;
            ViewBag.ShowUnsubscribedMessage = showUnsubscribedMessage;

            return View(chefViewModel);
        }

        [HttpGet] 
        public IActionResult ViewRecipeMarket()
        {
            AssignCurrentPageStatus(nameof(ViewRecipeMarket));
            var recipes = _recipeRepo.GetAll();
            return View(recipes);
        }

        [HttpGet]
        public IActionResult FilterRecipeMartetByRecipeType(string recipeType)
        {
            List<Recipe> recipes;

            if(recipeType == null || recipeType == "")
            {
                recipes = _recipeRepo.GetAll();
            }
            else
            {
                recipes = _recipeRepo.GetAll().Where(r => r.RecipeType == Enum.Parse<RecipeType>(recipeType)).ToList();
            }

            return PartialView("_RecipeMarketListPartial", recipes);
        }

        [HttpGet]
        public IActionResult FilterRecipeMartetByRecipeName(string searchedRecipeName)
        {
            List<Recipe> recipes;

            if (searchedRecipeName == null || searchedRecipeName == "")
            {
                recipes = _recipeRepo.GetAll();
            }
            else
            {
                recipes = _recipeRepo.GetAll().Where(r => r.Name.ToLower().Contains(searchedRecipeName.ToLower())).ToList();
            }

            return PartialView("_RecipeMarketListPartial", recipes);
        }

        [HttpPost]
        public IActionResult DismissNotification(int notificationId)
        {
            var notification = _notifySubscribers.GetById(notificationId);
            if (notification == null) return Json(new { success = false, message = "Error occured on dismissing notification" });

            _notifySubscribers.Delete(notification);

            _notifySubscribers.SaveChanges();

            return Json(new { success = true, message = "Notification dismissed" });
        }

        [HttpGet]
        public IActionResult ViewChefRestaurants()
        {
            AssignCurrentPageStatus("ViewChefRestaurants");
            return View();
        }

        #region Recipe Management

        [HttpGet]
        public IActionResult ViewRecipe(int recipeId)
        {
            AssignChefId();
            AssignCurrentPageStatus("");
            var recipe = _recipeRepo.GetById(recipeId);
            ViewBag.CurrentChefId = _chefId;
            return View(recipe);
        }

        public async Task<IActionResult> ManageRecipes(bool json = false)
        {
            AssignCurrentPageStatus("ManageRecipes");
            var chefVM = (await _chefRepo.GetChefsWithDetailsAsync()).Find(c => c.User.UserId == _currentUserId);
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
        public async Task<IActionResult> AddRecipe(int recipeId)
        {
            AssignCurrentPageStatus("");

            RecipeViewModel vm;
            if (recipeId == 0)
            {
                vm = new RecipeViewModel();
                vm.Instructions = new();
            }
            else
            {
                AssignChefId();
                var chef = (await _chefRepo.GetChefsWithDetailsAsync()).FirstOrDefault(c => c.ChefId == _chefId);
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

                Recipe recipe;

                if (model.RecipeId == 0) // Add new recipe
                {
                    // Create a new recipe based on the RecipeType
                    if (model.RecipeType == Models.Enums.RecipeType.Premium)
                    {
                        var premiumRecipe = new PremiumRecipe
                        {
                            Price = model.Price
                        };
                        recipe = premiumRecipe; // Treat it as a generic recipe after creation
                    }
                    else
                    {
                        recipe = new Recipe();
                    }

                    recipe.Name = model.Name!;
                    chef.Recipes.Add(recipe);

                    //create the notification
                    foreach (var subscriber in chef.PublishedSubscriptions)
                    {
                        var notification = new NotifySubscribers()
                        {
                            PublisherId = chef.ChefId,
                            SubscriberId = subscriber.SubscriberId,
                            RecipeId = recipe.RecipeId,
                            PublishedDate = DateTime.Now
                        };

                        _notifySubscribers.Add(notification);
                    }
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
                return RedirectToAction("ManageRecipes");
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

            var notifications = _notifySubscribers.GetAll().Where(ns => ns.RecipeId == recipeToRemove.RecipeId).ToList();

            notifications.ForEach(n =>
            {
                _notifySubscribers.Delete(n);
            });


            chef.Recipes.Remove(recipeToRemove);
            _chefRepo.Update(chef);

            TempData["RecipeSuccessMessage"] = "Recipe deleted successfully";
            TempData["RecipeSuccess"] = false;
            return RedirectToAction("ManageRecipes");
        }

        [HttpPost]
        public IActionResult AddInstruction(int recipeId, string recipeName, InstructionViewModel instruction, RecipeType recipeType, float? price)
        {
            AssignChefId();
            var chef = _chefRepo.GetById(_chefId);
            if (chef == null) throw new Exception($"Chef not found with chefId: {_chefId}");
            Recipe recipe;

            if (recipeId == 0)
            {
                // Create a new recipe based on RecipeType
                if (recipeType == RecipeType.Premium)
                {
                    recipe = new PremiumRecipe
                    {
                        Name = recipeName,
                        Price = price,
                        Instructions = new List<Instruction>()
                    };
                }
                else
                {
                    recipe = new Recipe
                    {
                        Name = recipeName,
                        Instructions = new List<Instruction>(),
                    };
                }

                chef.Recipes.Add(recipe);
                _chefRepo.Update(chef);
                //create the notification
                foreach (var subscriber in chef.PublishedSubscriptions)
                {
                    var notification = new NotifySubscribers()
                    {
                        PublisherId = chef.ChefId,
                        SubscriberId = subscriber.SubscriberId,
                        RecipeId = recipe.RecipeId,
                        PublishedDate = DateTime.Now
                    };

                    _notifySubscribers.Add(notification);
                }
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

        #region Review Management
        [HttpPost]
        public IActionResult AddReview(string reviewMessage, int recipeId, int chefId)
        {
            var recipe = _recipeRepo.GetById(recipeId);

            var reviewer = _chefRepo.GetById(chefId);

            Review newReview = new Review
            {
                Message = reviewMessage,
                ReviewerId = chefId,
                Reviewer = reviewer, // Assign the reviewer object
                RecipeId = recipeId,
                ReviewDate = DateTime.Now
            };

            recipe?.Reviews.Add(newReview);
            _recipeRepo.Update(recipe!);

            ReviewViewModel vm = new ReviewViewModel();
            vm.Reviews = recipe!.Reviews;
            return PartialView("_ViewRecipeReviews", vm.Reviews);
        }
        #endregion

        #region RECIPE & IMAGE AI

        #region RECIPE AI
        [HttpGet]
        public IActionResult RecipeSmartAI()
        {
            AssignCurrentPageStatus("RecipeSmartAI");
            var model = new PredictRecipeViewModel();
            //Thread.Sleep(millisecondsTimeout: 3000);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetRecipeListJson()
        {
            string filePath = Path.Combine("ML", "RecipeAI", "Data", "recipes_labelled.txt");

            //check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                return Json(new { success = false, message = "File not found" });
            }

            var lines = System.IO.File.ReadAllLines(filePath);

            var uniqueIngredients = lines.Skip(1) // Skip the header
                                   .SelectMany(line => line.Split(';').Take(5)) // Take first 5 columns as ingredients
                                   .Distinct()
                                   .OrderBy(s => s); // Get distinct ingredients



            Console.WriteLine($"length of uniqueIngredients: {uniqueIngredients.Count()}");
            return Json(new { success = true, recipeList = uniqueIngredients });
        }

        [HttpPost]
        public IActionResult Predict(PredictRecipeViewModel model)
        {
            // Manually remove the PredictedRecipe property from the ModelState
            if (!ModelState.IsValid)
            {
                // Return JSON response with error message
                return Json(new { success = false, message = "Please enter valid ingredients." });
            }

            var prediction = _recipeModel.Predict(model.Ingredients);
            model.PredictedRecipe = prediction.PredictedLabel!;

            var splittedPredictedLabel = prediction.PredictedLabel!.Split(" ");
            var recipes = _recipeRepo.GetAll();
            // Filter recipes where any of the words in splittedPredictedLabel is contained in the recipe's name
            var filteredRecipes = recipes.Where(r =>
                splittedPredictedLabel.Any(word => r.Name.Contains(word, StringComparison.OrdinalIgnoreCase))
            ).ToList(); 
            
            model.SuggestedRecipes = filteredRecipes;

            // Return the partial view with the prediction result
            return PartialView("_PredictionResultPartial", model);
        }
        #endregion

        #region IMAGE AI      
        [HttpGet]
        public IActionResult ImageSmartAI()
        {
            AssignCurrentPageStatus("ImageSmartAI");

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
                return Json(new { success = false});
            }

            //var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "ML", "ImageAI", "assets", "imagesNew");

            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            ViewBag.Message = "File uploaded successfully!";
            //var fileUrl = Path.Combine("/uploads", file.FileName);
            // Construct the URL path for the frontend
            var fileUrl = $"/imagesNew/{file.FileName}";
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

        [HttpPost]
        public IActionResult PredictImage(string url)
        {
            var imageName = url.Replace("/imagesNew/", "");
            var predictedImageName = _imageModel.ClassifySingleImage(imageName);
            return Json(new { predictedImageName });
        }


        [HttpGet]
        public async Task<IActionResult> GetRecipeViewPartial(int recipeId)
        {
            var recipe = _recipeRepo.GetById(recipeId);
            return PartialView("_ViewRecipePartial", recipe);
        }
        #endregion

        #endregion

        //Used to convert PartialView to string in order to send in a JSON object
        public async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.ActionDescriptor.ActionName;
            }

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"View '{viewName}' not found.");
                }

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
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

        private ChefViewModel GetCurrentChef()
        {
            var chef = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.User.UserId == _currentUserId);
            return chef;
        }
        private void AssignCurrentPageStatus(string currentPageName)
        {
            ViewBag.CurrentActive = currentPageName;
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            ViewBag.CurrentUserBalance = HttpContext.Session.GetObjectFromJson<string>("CurrentUserBalance");
        }
    }
}
