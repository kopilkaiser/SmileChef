using Microsoft.AspNetCore.Mvc;

namespace SmileChef.Controllers
{
    [Route("/chefs")]
    public class ChefController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/chefs/info")]
        public IActionResult Info()
        {
            var jsonString = "{\"name\":\"Rahul Verma\", \"age\":34, \"isAdult\":true}";

            return Content(jsonString, "application/json");
        }

        [Route("/chefs/infov2")]
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
    }
}
