using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Models.ViewModels;
using IssueTracker.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IssueTrackerWeb.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeViewModel HomeViewModel { get; set; }

        public HomeController(
            ILogger<HomeController> logger, 
            SignInManager<IdentityUser> signInManager, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeViewModel = new HomeViewModel()
            {
                Issues = _unitOfWork.Issue.GetAll(),
                Comments = _unitOfWork.Comment.GetAll(includeProperties: "User").OrderByDescending(x => x.CreatedDate),
                PriorityCount = new Dictionary<string, int>()
            };

            // populate the dictionary with priority keys
            HomeViewModel.PriorityCount.Add(Globals.ISSUE_PRIORITY_CRITICAL, 0);
            HomeViewModel.PriorityCount.Add(Globals.ISSUE_PRIORITY_IMPORTANT, 0);
            HomeViewModel.PriorityCount.Add(Globals.ISSUE_PRIORITY_STANDARD, 0);
            HomeViewModel.PriorityCount.Add(Globals.ISSUE_PRIORITY_LOW, 0);
            HomeViewModel.PriorityCount.Add(Globals.ISSUE_PRIORITY_NOTASSIGNED, 0);

            // populate the dictionary with priority values
            for (int i = 0; i < HomeViewModel.Issues.Count(); i++)
            {
                if(HomeViewModel.Issues.ElementAt(i).Priority == Globals.ISSUE_PRIORITY_NOTASSIGNED)
                {
                    HomeViewModel.PriorityCount[Globals.ISSUE_PRIORITY_NOTASSIGNED]++;
                }
                else if(HomeViewModel.Issues.ElementAt(i).Priority == Globals.ISSUE_PRIORITY_LOW)
                {
                    HomeViewModel.PriorityCount[Globals.ISSUE_PRIORITY_LOW]++;
                }
                else if (HomeViewModel.Issues.ElementAt(i).Priority == Globals.ISSUE_PRIORITY_STANDARD)
                {
                    HomeViewModel.PriorityCount[Globals.ISSUE_PRIORITY_STANDARD]++;
                }
                else if (HomeViewModel.Issues.ElementAt(i).Priority == Globals.ISSUE_PRIORITY_IMPORTANT)
                {
                    HomeViewModel.PriorityCount[Globals.ISSUE_PRIORITY_IMPORTANT]++;
                }
                else
                {
                    HomeViewModel.PriorityCount[Globals.ISSUE_PRIORITY_CRITICAL]++;
                }
            }

            return View(HomeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> MockLogin(string role)
        {
            HttpContext.Session.SetString("login_role", "");

            string name = "";
            string password = "";
            switch (role)
            {
                case "admin":
                    name = "admin@issuetracker";
                    password = "7EuQTsJt8Dbyw4q+";
                    break;
                case "advanceduser":
                    name = "advanceduser@issuetracker";
                    password = "7EuQTsJt8Dbyw4q+";
                    break;
                case "basicuser":
                    name = "basicuser@issuetracker";
                    password = "7EuQTsJt8Dbyw4q+";
                    break;
                case "guest":
                    HttpContext.Session.SetString("login_role", Globals.ROLE_USER_GUEST);
                    break;
                default:
                    break;
            }

            Microsoft.AspNetCore.Identity.SignInResult result = 
                await _signInManager.PasswordSignInAsync(name, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["success"] = "Login successful.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}