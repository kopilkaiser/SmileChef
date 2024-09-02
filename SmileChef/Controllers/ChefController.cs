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
using Google.Protobuf.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmileChef.Controllers
{
    public class ChefController : Controller
    {
        #region GLOBAL VARIALBES
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChefController> _logger;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly RecipeSmartModel _recipeModel;
        private readonly ImageSmartModel _imageModel;

        private static int? _currentUserId;
        private int _chefId;

        #region Repositories
        private readonly IChefRepository _chefRepo;
        private readonly IRepository<Instruction> _instructRepo;
        private readonly IRepository<Recipe> _recipeRepo;
        private readonly IRepository<Subscription> _subRepo;
        private readonly IRepository<NotifySubscribers> _notifySubscribers;
        private readonly IRepository<Review> _reviewRepo;
        private readonly IRepository<RecipeBookmark> _bookmarkRepo;
        private readonly IRepository<Order> _orderRepo;
        #endregion

        #endregion

        public ChefController(ILogger<ChefController> logger, IHttpContextAccessor httpContextAccessor, IChefRepository chefRepo, IRepository<Instruction> instructRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subRepo, IWebHostEnvironment webHostEnvironment, RecipeSmartModel recipeModel, IRepository<NotifySubscribers> notifySubscribers, IRepository<Review> reviewRepo, IRepository<RecipeBookmark> bookmarkRepo, IConfiguration config, ICompositeViewEngine viewEngine, IRepository<Order> orderRepo)
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
            _bookmarkRepo = bookmarkRepo;
            _config = config;
            _viewEngine = viewEngine;
            AssignChefAndUserId();
            _orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            AssignCurrentPageStatus("ChefIndex");
            var viewModel = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.User.UserId == _currentUserId);
            var userNotifications = _notifySubscribers.GetAll().Where(n => n.SubscriberId == viewModel.ChefId);
            if (userNotifications.Count() > 0) viewModel.Notifications = userNotifications.ToList();
            else viewModel.Notifications = new List<NotifySubscribers>();
            return View("ChefIndex", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowChefCards(bool showSubscriptionModal = false, bool showUnsubscribedMessage = false)
        {
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
            if (Enum.TryParse<SubscriptionType>(query.GetProperty("subType").GetString(), out var subType))
            {
                subscription.SubscriptionType = subType;
            }

            _subRepo.Add(subscription);
            return Json(new { success = true });
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

        // #Todo: Add "Restaurant" Entity to hold details of Chef's Restaurants
        [HttpGet]
        public IActionResult ViewChefRestaurants()
        {
            AssignCurrentPageStatus("ViewChefRestaurants");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAmountToBalance(decimal amountToAdd)
        {
            var chef = _chefRepo.GetById(_chefId);
            chef.AccountBalance += amountToAdd;

            if(chef.AccountBalance > 999999)
            {
                return Json(new
                {
                    success = false,
                    message = $"Your total balance cannot exceed £999,999",
                    accountBalance = chef.AccountBalance
                });
            }

            _chefRepo.Update(chef);
            UpdateChefBalance(chef.AccountBalance);
            return Json(new
            {
                success = true,
                message = $"<p class=\"mb-2\">Amount <b class=\"text-success fs-5\">£{amountToAdd}</b> added successfully! </p>" +
                $"<p>Your new Balance is <b class=\"text-success fs-5\">£{chef.AccountBalance}</b></p",
                accountBalance  = chef.AccountBalance
            });
        }

        #region Recipe Market

        [HttpGet]
        public IActionResult ViewRecipeMarket()
        {
            AssignCurrentPageStatus(nameof(ViewRecipeMarket));
            var recipes = _recipeRepo.GetAll();
            RecipeMarketViewModel vm = new RecipeMarketViewModel();
            vm.Recipes = recipes;
            vm.CurrentBookmarks = _bookmarkRepo.GetAll().Where(bm => bm.ChefId == _chefId).ToList();
            vm.CurrentCHef = _chefRepo.GetById(_chefId);
            return View(vm);
        }

        [HttpGet]
        public IActionResult FilterRecipeMartetByRecipeType(string recipeType)
        {
            List<Recipe> recipes;

            if (recipeType == null || recipeType == "")
            {
                recipes = _recipeRepo.GetAll();
            }
            else
            {
                recipes = _recipeRepo.GetAll().Where(r => r.RecipeType == Enum.Parse<RecipeType>(recipeType)).ToList();
            }
            RecipeMarketViewModel vm = new RecipeMarketViewModel();
            vm.Recipes = recipes;
            vm.CurrentBookmarks = _bookmarkRepo.GetAll().Where(bm => bm.ChefId == _chefId).ToList();
            vm.CurrentCHef = _chefRepo.GetById(_chefId);

            return PartialView("_RecipeMarketListPartial", vm);
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
            RecipeMarketViewModel vm = new RecipeMarketViewModel();
            vm.Recipes = recipes;
            vm.CurrentBookmarks = _bookmarkRepo.GetAll().Where(bm => bm.ChefId == _chefId).ToList();
            vm.CurrentCHef = _chefRepo.GetById(_chefId);

            return PartialView("_RecipeMarketListPartial", vm);
        }

        #endregion

        #region Recipe Management

        [HttpGet]
        public IActionResult ViewRecipe(int recipeId)
        {
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
                return Json(new { success = false });
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

        #region Recipe Bookmark
        // #Todo : Show Recipe bookmark list, And Be able to delete bookmarks
        [HttpGet]
        public IActionResult GetRecipeBookmarks()
        {
            AssignCurrentPageStatus("GetRecipeBookmarks");
            var recipeBookmarks = _bookmarkRepo.GetAll().Where(bm => bm.ChefId == _chefId).ToList();
            return View(recipeBookmarks);
        }

        [HttpPost]
        public IActionResult AddOrRemoveRecipeBookmark(int recipeId)
        {
            AssignCurrentPageStatus("GetRecipeBookmarks");

            var recipeBookmark = _bookmarkRepo.GetAll().FirstOrDefault(bm => bm.RecipeId == recipeId && bm.ChefId == _chefId);

            if (recipeBookmark == null)
            {
                RecipeBookmark rb = new RecipeBookmark(recipeId, _chefId);
                _bookmarkRepo.Add(rb);
                return Json(new { success = true, status = "added" });

            }
            else
            {
                _bookmarkRepo.Delete(recipeBookmark);
                return Json(data: new { success = true, status = "deleted" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRecipeBookmarkById(int bookmarkId)
        {
            AssignCurrentPageStatus("GetRecipeBookmarks");
            var recipeBookmark = _bookmarkRepo.GetById(bookmarkId);
            _bookmarkRepo.Delete(recipeBookmark);
            var bookMarks = _bookmarkRepo.GetAll().Where(b => b.ChefId == _chefId).ToList();
            var view = await RenderPartialViewToStringAsync("_RecipeBookmarkPartial", bookMarks);
            return Json(new { success = true, partialView = view });
        }
        #endregion

        #region Order Management

        [HttpPost]
        public async Task<IActionResult> GetCurrentCart()
        {
            var currentCart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<Order>("CurrentOrder");

            if(currentCart == null)
            {
                return Json(new { });
            }

            var view = await RenderPartialViewToStringAsync("_OrderDetailsPartial", currentCart);
            return Json(new { partialView = view });
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderLine(int aRecipeId, int aQuantity)
        {
            var recipe = _recipeRepo.GetById(aRecipeId) as PremiumRecipe;

            if (recipe == null)
            {
                return Json(new { success = false, message = $"Recipe with Id: {aRecipeId} is not found" });
            }

            if (aQuantity <= 0)
            {
                return Json(new { success = false, message = "Quantity cannot be less than or equal to zero" });
            }
            else if (aQuantity > 999)
            {
                return Json(new { success = false, message = $"Quantity cannot be greater than 999" });
            }

            var existingOrder = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<Order>("CurrentOrder");
            if (existingOrder == null)
            {
                existingOrder = new Order
                {
                    ChefId = _chefId,
                    OrderDate = DateTime.Now
                };
            }

            // Create a new OrderLine object for each addition
            var newOrderLine = new OrderLine
            {
                RecipeName = recipe.Name,
                RecipeId = recipe.RecipeId,
                Quantity = aQuantity,
                Price = Convert.ToDecimal(recipe.Price) // Assuming Price is per unit
            };

            // Add or update the OrderLine in the Order
            var response = existingOrder.AddOrUpdateOrderLine(newOrderLine);

            if (!response.success)
            {
                return Json(new { response.success, response.message });
            }

            // Save the updated order back into the session.
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("CurrentOrder", existingOrder);

            // Render the order details partial view and return it as a JSON response.
            var view = await RenderPartialViewToStringAsync("_OrderDetailsPartial", existingOrder);
            return Json(new { success = true, partialView = view, message = "Selected Recipe has been added to Cart" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrderLine(int indexOfOrderLine)
        {
            // Retrieve the existing order from the session.
            var existingOrder = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<Order>("CurrentOrder");

            if (existingOrder == null || !existingOrder.OrderLines.Any())
            {
                return Json(new { success = false, message = "No order found or the order is empty." });
            }

            // Check if the index is within the range of the OrderLines list
            if (indexOfOrderLine < 0 || indexOfOrderLine >= existingOrder.OrderLines.Count)
            {
                return Json(new { success = false, message = "Invalid order line index." });
            }

            // Remove the order line using the index
            var removed = existingOrder.RemoveOrderLine(indexOfOrderLine);
            if (!removed)
            {
                return Json(new { success = false, message = "Failed to remove the order line." });
            }

            // Update the session with the modified order.
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("CurrentOrder", existingOrder);

            // Optionally, return the updated partial view to reflect changes in the UI.
            var view = await RenderPartialViewToStringAsync("_OrderDetailsPartial", existingOrder);
            return Json(new { success = true, partialView = view, message = "Item has been removed successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutOrder()
        {
            // Retrieve the existing order from the session.
            var existingOrder = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<Order>("CurrentOrder");

            if (existingOrder == null || !existingOrder.OrderLines.Any())
            {
                return Json(new { success = false, message = "There are no items in the cart to checkout." });
            }

            var chef = _chefRepo.GetById(_chefId);

            if (existingOrder.TotalPrice > chef.AccountBalance)
            {
                return Json(new { success = false, message = "You have insufficient balance. Please Top-Up Balance" });
            }

            // Deduct the total price from the chef's balance and update the database.
            chef.AccountBalance -= existingOrder.TotalPrice;
            _chefRepo.Update(chef);

            // Update the session with the new balance and update ViewBag.
            UpdateChefBalance(chef.AccountBalance);

            existingOrder.OrderDate = DateTime.Now;
            // Save the existing order to the database.
            _orderRepo.Add(existingOrder);

            // Clear the session or replace it with a new empty order.
            _httpContextAccessor.HttpContext.Session.Remove("CurrentOrder");

            // Optionally, start a new order and update the session.
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("CurrentOrder", new Order
            {
                ChefId = _chefId,
            });

            var order = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<Order>("CurrentOrder");

            var view = await RenderPartialViewToStringAsync("_OrderDetailsPartial", order);

            return Json(new { success = true, partialView = view, message = $"" +
                $"<p class=\"mb-2 fs-4 fw-semibold\">Your order has been placed <i class=\"fa-solid fa-truck ps-1 text-primary\"></i></p>" +
                $"<p class=\"fw-semibold mb-2 \">Remaining Balance: <b class=\"text-success fs-5\">{chef.AccountBalance:C2}</b></p>" + 
                $"<p class=\"fs-5\">Tasty Recipes will be at your door soon !</p>", accountBalance = chef.AccountBalance });
        }

        #endregion


        #region Helper Methods

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

        private void AssignChefAndUserId()
        {
            _currentUserId = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<int>("CurrentUserId");
            if (_currentUserId == 0)
            {
                _currentUserId = _config.GetValue<int>("CurrentUserId"); // Default user ID if not set
                _httpContextAccessor.HttpContext?.Session.SetObjectAsJson("CurrentUserId", _currentUserId);

            }

            _chefId = _httpContextAccessor.HttpContext?.Session.GetInt32("CurrentChefId") ?? 0;
            if (_chefId == 0)
            {
                var chef = _chefRepo.GetAll().FirstOrDefault(c => c.UserId == _currentUserId);
                _chefId = chef.ChefId;
                _httpContextAccessor.HttpContext?.Session.SetInt32("CurrentChefId", _chefId);
            }
        }

        private ChefViewModel GetCurrentChef()
        {
            var chef = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.User.UserId == _currentUserId);
            return chef;
        }

        private void UpdateChefBalance(decimal? balance)
        {
            HttpContext.Session.SetObjectAsJson("CurrentUserBalance", balance);
            ViewBag.CurrentUserBalance = HttpContext.Session.GetObjectFromJson<string>("CurrentUserBalance");
        }

        private void AssignCurrentPageStatus(string currentPageName)
        {
            var chefVM = _chefRepo.GetChefsWithDetails().Find(c => c.User.UserId == _currentUserId);
            if (HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail") == null)
            {
                HttpContext.Session.SetObjectAsJson("CurrentUserEmail", chefVM.User.Email);
                HttpContext.Session.SetObjectAsJson("CurrentUserName", chefVM.ChefName);
                HttpContext.Session.SetObjectAsJson("CurrentUserBalance", chefVM.AccountBalance);
            }
            ViewBag.CurrentActive = currentPageName;
            ViewBag.CurrentUserEmail = HttpContext.Session.GetObjectFromJson<string>("CurrentUserEmail");
            ViewBag.CurrentUserName = HttpContext.Session.GetObjectFromJson<string>("CurrentUserName");
            ViewBag.CurrentUserBalance = HttpContext.Session.GetObjectFromJson<decimal>("CurrentUserBalance");
        }

        #endregion

        #region Methods for example purpose
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

        #endregion
    }
}
