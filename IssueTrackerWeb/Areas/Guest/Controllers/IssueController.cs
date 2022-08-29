using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Models.ViewModels;
using IssueTracker.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace IssueTrackerWeb.Controllers
{
    [Area("Guest")]
    public class IssueController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public IssueViewModel IssueViewModel { get; set; }

        public IssueController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Issue> objIssueList = _unitOfWork.Issue.GetAll();
            return View(objIssueList);
        }

        // Get
        public IActionResult Create()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Issue obj)
        {
            if (ModelState.IsValid)
            {
                // add non-inputable properties
                obj.CreatedDate = DateTime.Now;
                obj.LastUpdated = DateTime.Now;

                _unitOfWork.Issue.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Issue created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var objFromDb = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id);
            if(objFromDb == null)
            {
                return NotFound();
            }

            return View(objFromDb);
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Issue obj)
        {
            if (ModelState.IsValid)
            {
                obj.LastUpdated = DateTime.Now;

                _unitOfWork.Issue.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Issue edited successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get
        public IActionResult Details(int? id, string? param)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

			switch (param)
			{
                case "details":
                    IssueViewModel = new IssueViewModel()
                    {
                        IssueId = id,
                        Parameter = param,
                        Issue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id)
                    };
                    if (IssueViewModel.Issue == null)
                    {
                        return NotFound();
                    }
                    return View(IssueViewModel);

                case "comments":
                    IssueViewModel = new IssueViewModel()
                    {
                        IssueId = id,
                        Parameter = param,
                        Comments = _unitOfWork.Comment.GetAll(x => x.IssueId == id, includeProperties: "User")
                                        .OrderByDescending(x => x.CreatedDate)
                    };
                    return View(IssueViewModel);

                case "history":
                    IssueViewModel = new IssueViewModel()
                    {
                        IssueId = id,
                        Parameter = param,
                        Comments = _unitOfWork.Comment.GetAll(x => x.IssueId == id, includeProperties: "User")
                    };
                    return View(IssueViewModel);

                case "assignedto":
                    var AllUsers = _unitOfWork.User.GetAll();
                    var IssueUsers = _unitOfWork.IssueAssignment.GetAll(x => x.IssueId == id);
                    Dictionary<string, bool> CheckedList = new();
                    foreach(var user in AllUsers)
                    {
                        CheckedList[user.UserName] = false;
                        foreach (var issue in IssueUsers)
                        {
                            if(issue.IssueId == id && issue.UserId == user.Id)
                            {
                                CheckedList[user.UserName] = true;
                            }
                        }
                    }

                    IssueViewModel = new IssueViewModel()
                    {
                        IssueId = id,
                        Parameter = param,
                        AssignIssueViewModel = new AssignIssueViewModel
                        {
                            IssueId = id,
                            UserList = CheckedList
                        }
                    };
                    return View(IssueViewModel);

                default:
                    return View();
            }
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsUpdate(IssueViewModel obj)
        {
            var objFromDb = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == obj.IssueId);
            if (objFromDb == null)
            {
                return NotFound();
            }

			if (ModelState.IsValid)
			{
                objFromDb.Description = obj.Issue.Description;
                objFromDb.LastUpdated = DateTime.Now;
                objFromDb.Status = obj.Issue.Status;
                if(obj.Issue.Status == Globals.ISSUE_STATUS_RESOLVED)
                {
                    objFromDb.Priority = Globals.ISSUE_PRIORITY_NOTASSIGNED;
                    var issueAssignmentFromDb = _unitOfWork.IssueAssignment.GetAll(x => x.IssueId == objFromDb.Id);
                    _unitOfWork.IssueAssignment.RemoveRange(issueAssignmentFromDb);
                }
                else
                {
                    switch (obj.Issue.Priority)
                    {
                        case "1":
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_LOW;
                            break;
                        case "2":
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_STANDARD;
                            break;
                        case "3":
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_IMPORTANT;
                            break;
                        case "4":
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_CRITICAL;
                            break;
                    }
                }

                _unitOfWork.Issue.Update(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Changes saved succesfully.";
                return RedirectToAction("Details", controllerName: "Issue", new { id = obj.IssueId, param = obj.Parameter });
            }

            TempData["error"] = "An error occured while saving changes.";
            return RedirectToAction("Details", controllerName: "Issue", new { id = obj.IssueId, param = obj.Parameter });
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsAssignIssues(IssueViewModel obj)
        {
            foreach(var checkbox in obj.AssignIssueViewModel.UserList)
            {
                var userId = _unitOfWork.User.GetFirstOrDefault(x => x.UserName == checkbox.Key).Id;
                var objFromDb = _unitOfWork.IssueAssignment.GetFirstOrDefault(x => x.IssueId == obj.IssueId && x.UserId == userId);

                if (checkbox.Value)
                {
                    // if an entry in the IssueAssignment table doesn't exist and the checkbox is checked,
                    // then create an entry
                    if (objFromDb == null)
                    {
                        _unitOfWork.IssueAssignment.Add(new IssueAssignment
                        {
                            IssueId = (int)obj.IssueId,
                            UserId = userId
                        });
                    }
                }
                else
                {
                    // if an entry in the IssueAssignment table exists and the checkbox is not checked,
                    // then delete the entry
                    if (objFromDb != null)
                    {
                        _unitOfWork.IssueAssignment.Remove(objFromDb);
                    }
                }
            }

            _unitOfWork.Save();
            TempData["success"] = "Changes saved succesfully.";

            return RedirectToAction("Details", controllerName: "Issue", new { id = obj.IssueId, param = obj.Parameter });
        }

            // API calls
            #region API CALLS
            [HttpGet]
        public IActionResult GetAll()
        {
            var issueList = _unitOfWork.Issue.GetAll();
            return Json(new { data = issueList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDb = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Issue.Remove(objFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Issue deleted succesfully.";
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
