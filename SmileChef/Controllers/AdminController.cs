using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmileChef.Extensions;
using SmileChef.Models.DbModels;
using SmileChef.Repository;
using Tensorflow;

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
           
            var user = GetCurrentAdminUser();
            return View(user);
        }

        [HttpGet]
        public IActionResult ShowIssues()
        {
            var issues = _supportRepo.GetAll();
            List<SelectListItem> selectItems = Enum.GetValues<SupportStatus>().Select(s => new SelectListItem()
            {
                Text = s.ToString(),
                Value = s.ToString()
            }).ToList();
            selectItems.Insert(0, new SelectListItem() { Text = "Show All", Value = "" });
            ViewBag.SelectStatusList = selectItems;
            return View(issues);
        }

        [HttpPost]
        public IActionResult ResoleIssue(int supportMessageId, string adminMessage)
        {
            var getSupportMessage = _supportRepo.GetById(supportMessageId);
            getSupportMessage.AdminUser = GetCurrentAdminUser();
            getSupportMessage.AdminMessage = adminMessage;
            getSupportMessage.SupportStatus = SupportStatus.Resolved;
            getSupportMessage.ClosedDate = DateTime.Now;
            _supportRepo.Update(getSupportMessage);
            var supports = _supportRepo.GetAll();

            return PartialView("_SupportMessagesPartial", supports);
        }

        [HttpPost]
        public IActionResult OpenIssue(int supportMessageId)
        {
            var getSupportMessage = _supportRepo.GetById(supportMessageId);
            getSupportMessage.AdminUser = null;
            getSupportMessage.AdminMessage = null;
            getSupportMessage.SupportStatus = SupportStatus.Ongoing;
            getSupportMessage.Created = DateTime.Now;
            _supportRepo.Update(getSupportMessage);
            var supports = _supportRepo.GetAll();
            return PartialView("_SupportMessagesPartial", supports);
        }

        [HttpPost]
        public IActionResult FilterIssuesByEmail(string senderEmail)
        {
            List<SupportMessage> supports;
            var allSupports = _supportRepo.GetAll();

            if (string.IsNullOrEmpty(senderEmail))
            {
                supports = allSupports;
            }
            else
            {
                supports = allSupports.Where(sm => sm.Sender.User.Email.ToLower().Contains(senderEmail.ToLower())).ToList();
            }

            return PartialView("_SupportMessagesPartial", supports);
        }

        [HttpPost]
        public IActionResult FilterIssuesByStatus(string status)
        {
            List<SupportMessage> supports;
            var allSupports = _supportRepo.GetAll();

            if (string.IsNullOrEmpty(status))
            {
                supports = allSupports;
            }
            else
            {
                var parsedStatus = Enum.Parse<SupportStatus>(status);
                supports = allSupports.Where(sm => sm.SupportStatus == parsedStatus).ToList();
            }

            return PartialView("_SupportMessagesPartial", supports);
        }

        private User GetCurrentAdminUser()
        {
            var currentUserId = HttpContext?.Session.GetInt32("CurrentChefId");
            User currentUser;
            if(HttpContext.Session.GetObjectFromJson<User>("CurrentAdmin") == null)
            {
                currentUser = _chefRepo.GetAll().FirstOrDefault(c => c.UserId == currentUserId).User;
                HttpContext.Session.SetObjectAsJson("CurrentAdmin", currentUser);
            }
            else
            {
                currentUser = HttpContext.Session.GetObjectFromJson<User>("CurrentAdmin");
            }

            return currentUser;
        }
    }
}
