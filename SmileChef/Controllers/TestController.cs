using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmileChef.Extensions;
using SmileChef.Repository;
using System.Text.Json;

namespace SmileChef.Controllers
{
    [Route("/tests")]
    public class TestController : Controller
    {
        private const string ChefsSessionKey = "ChefsNew";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChefController> _logger;
        private readonly IChefRepository _chefRepo;
        private List<ChefNew>? _chefsNew;
        private IRepository<Instruction> _instructRepo;
        private IRepository<Recipe> _recipeRepo;
        private IRepository<Subscription> _subRepo;

        public TestController(ILogger<ChefController> logger, IHttpContextAccessor httpContextAccessor, IChefRepository chefRepo, IRepository<Instruction> instructRepo, IRepository<Recipe> recipeRepo, IRepository<Subscription> subRepo)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _chefRepo = chefRepo;
            _instructRepo = instructRepo;
            _recipeRepo = recipeRepo;
            _subRepo = subRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("info")] // Will only be accessible via GET requests
        //[Route("info")]   // Will be accessible via any HTTP method (GET, POST, PUT, DELETE, etc.)
        //[Route("/chefs/info")] // Absolute routing: also valid will be used for all (GET,POST,PUT,DELETE etc) requests
        public IActionResult Info()
        {
            var jsonString = "{\"name\":\"Rahul Verma\", \"age\":34, \"isAdult\":true}";

            return Content(jsonString, "application/json");
        }

        [HttpGet("infov2")] // Will only be accessible via GET requests
        public IActionResult InfoV2()
        {
            var chefInfo = new
            {
                Name = "Rahul Verma",
                Age = 34,
                IsAdult = true
            };

            return new JsonResult(new { name = "Tash Shouheski", Age = 25, IsProgrammer = true })
            {
                ContentType = "application/json"
            };
        }

        [HttpGet("testShowChefs")]
        public IActionResult ShowChefs()
        {
            var chefsWithDetails = _chefRepo.GetChefsWithDetails();
            var chefs = _chefRepo.GetAll();
            var insturctions = _instructRepo.GetAll();
            var recipes = _recipeRepo.GetAll();
            var subs = _subRepo.GetAll();

            try
            {
                // Retrieve the chefs list from the session
                _chefsNew = HttpContext.Session.GetObjectFromJson<List<ChefNew>>(ChefsSessionKey);

                // If the list is not in session, initialize it and store it in the session
                if (_chefsNew == null)
                {
                    _chefsNew = new List<ChefNew>
                {
                    new ChefNew("John Carma", 4.0f, "Burger Chef"),
                    new ChefNew("Samantha Nivelle", 2.5f, "Sauce Chef"),
                    new ChefNew("Sterling Toot", 5.0f, "Steak Chef"),
                    new ChefNew("Tonny Kakkar", 3.0f, "Curry Chef"),
                    new ChefNew("Mohsin Ahmed", 1.0f, "Salad Chef"),
                    new ChefNew("Rony Chowdhury", 4.5f, "Sandwich Chef"),
                };
                    HttpContext.Session.SetObjectAsJson(ChefsSessionKey, _chefsNew);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving or initializing chefs data.");
                return StatusCode(500, "Internal server error");
            }

            return Ok(_chefsNew);
        }

        [HttpGet("animals/{id}")]
        public IActionResult DisplayAnimals(int id, string animalName, int[] listOfNumbers)
        {
            //returning 'text/plain'
            //return Ok("Success: retrieved data");

            //returning 'html'
            //return Content("<h3>Success: retrieved data. H3 Header</h3><br><p>this is a paragraph</p>", "application/json");

            var request = HttpContext.Request;
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var requestType = request.Method;
            //returning JSON
            var data = new
            {
                Message = "Success: retrieved data.",
                Header = "H3 Header",
                Paragraph = "This is a paragraph"
            };
            return Json(data);
        }

        [HttpPost("animals")]
        public IActionResult DisplayAnimals([FromBody] JsonElement query)
        {
            //when using JsonElement type
            //string animalNameGiven = query.GetProperty("animalName").GetString()!;
            //var nums = query.GetProperty("listOfNumbers").Deserialize<int[]>();
     

            string animalNameGiven = query.GetProperty("animalName").GetString()!;

            var userObj = query.GetProperty(propertyName: "user");
            var userName = userObj.GetProperty("name").GetString();
            var userContacts = userObj.GetProperty("listOfContacts").Deserialize<int[]>();
            var listOfNumbers = query.GetProperty("listOfNumbers").Deserialize<int[]>();



            //when using JObject type
            //var animalNameGiven = query["animalName"]!.ToString();
            //var listOfNumGiven = query["listOfNumbers"]!.ToObject<int[]>();

            //if used dynamic in junction with NewtonsoftJson extension method of Microsoft.AspNetCore.Mvc.NewtonsoftJson (8.0.6)
            //var animalNameGiven = (string) query.animalName;
            //var listOfNumbersGiven = ((JArray)query.listOfNumbers).ToObject<int[]>();

            List<Animal> animals = new List<Animal>();
            animals.Add(new Animal() { Id = 1, Name = "Lion" });
            animals.Add(new Animal() { Id = 2, Name = "Tiger" });
            animals.Add(new Animal() { Id = 3, Name = "Cheetah" });

            return PartialView("_DisplayAnimals", animals);
        }
    }

    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ChefNew
    {
        private static int incrementalId;
        private static readonly object idLock = new object();
        public int Id { get => incrementalId; private set { } }
        public string? Name { get; set; }
        public float Star { get; set; }

        public string? Specialist { get; set; }

        public ChefNew(string name, float star, string specialist)
        {
            lock (idLock)
            {
                Id = ++incrementalId;
            }

            Name = name;
            Star = star;
            Specialist = specialist;
        }
    }
}
