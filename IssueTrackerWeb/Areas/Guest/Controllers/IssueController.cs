using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Models.ViewModels;
using IssueTracker.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
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

                // create a history record
                // has to be called after adding an issue, otherwise
                // the call crashes looking for IssueId of a non-existing Issue
                AddIssueHistory(obj, "Issue created.");

                // double Save() call only in this method, everywhere else
                // the Issue already exists
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
                                        .OrderByDescending(x => x.CreatedDate),
                        Issue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id)
                    };

                    if (IssueViewModel.Issue == null)
                    {
                        return NotFound();
                    }
                    return View(IssueViewModel);

                case "history":
                    IssueViewModel = new IssueViewModel()
                    {
                        IssueId = id,
                        Parameter = param,
                        Histories = _unitOfWork.IssueHistory.GetAll(x => x.IssueId == id)
                                        .OrderByDescending(x => x.CreatedDate),
                        Issue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id)
                    };

                    if (IssueViewModel.Issue == null)
                    {
                        return NotFound();
                    }
                    return View(IssueViewModel);

                case "assignedto":
                    var currentIssue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id);
                    if (currentIssue == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        // in case someone decides to type "assignedto" directly into the URL,
                        // return just a barebones model
                        if(currentIssue.Status == "Resolved")
                        {
                            IssueViewModel = new IssueViewModel()
                            {
                                IssueId = id,
                                Parameter = param,
                                Issue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id)
                            };

                            return View(IssueViewModel);
                        }
                        // normal proceedings
                        else
                        {
                            var allUsers = _unitOfWork.User.GetAll();
                            var issueUsers = _unitOfWork.IssueAssignment.GetAll(x => x.IssueId == id);
                            Dictionary<string, bool> CheckedList = new();
                            foreach (var user in allUsers)
                            {
                                CheckedList[user.UserName] = false;
                                foreach (var issue in issueUsers)
                                {
                                    if (issue.IssueId == id && issue.UserId == user.Id)
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
                                },
                                Issue = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id)
                            };

                            return View(IssueViewModel);
                        }
                    }

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
                // prepare a history record string
                string historyDescription = "";

                // update the issue
                objFromDb.Description = obj.Issue.Description;
                objFromDb.LastUpdated = DateTime.Now;

                // prevent duplicate history records
                if(objFromDb.Status != obj.Issue.Status)
                {
                    historyDescription += "Status changed to " + obj.Issue.Status + ". ";
                }
                objFromDb.Status = obj.Issue.Status;

                // if an issue is resolved -> remove assignments
                if(obj.Issue.Status == Globals.ISSUE_STATUS_RESOLVED)
                {
                    objFromDb.Priority = Globals.ISSUE_PRIORITY_NOTASSIGNED;
                    var issueAssignmentFromDb = _unitOfWork.IssueAssignment.GetAll(x => x.IssueId == objFromDb.Id);
                    _unitOfWork.IssueAssignment.RemoveRange(issueAssignmentFromDb);

                    historyDescription = "Status changed to " + objFromDb.Status + ". ";
                }
                else
                {
                    switch (obj.Issue.Priority)
                    {
                        case "1":
                            // the switch being int based because of the priority slider element
                            // makes these checks very clunky
                            if(objFromDb.Priority != Globals.ISSUE_PRIORITY_LOW)
                            {
                                historyDescription += "Priority changed to " + Globals.ISSUE_PRIORITY_LOW + ". ";
                            }
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_LOW;
                            break;

                        case "2":
                            if (objFromDb.Priority != Globals.ISSUE_PRIORITY_STANDARD)
                            {
                                historyDescription += "Priority changed to " + Globals.ISSUE_PRIORITY_STANDARD + ". ";
                            }
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_STANDARD;
                            break;

                        case "3":
                            if (objFromDb.Priority != Globals.ISSUE_PRIORITY_IMPORTANT)
                            {
                                historyDescription += "Priority changed to " + Globals.ISSUE_PRIORITY_IMPORTANT + ". ";
                            }
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_IMPORTANT;
                            break;

                        case "4":
                            if (objFromDb.Priority != Globals.ISSUE_PRIORITY_CRITICAL)
                            {
                                historyDescription += "Priority changed to " + Globals.ISSUE_PRIORITY_CRITICAL + ". ";
                            }
                            objFromDb.Priority = Globals.ISSUE_PRIORITY_CRITICAL;
                            break;
                    }
                }

                // create a history record
                AddIssueHistory(objFromDb, historyDescription);

                // finalize updating
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

        // get
        public IActionResult MyIssues()
        {
            return View("MyIssues");
        }

        private void AddIssueHistory(Issue issue, string description)
        {
            if(description != "")
            {
                IssueHistory history = new IssueHistory
                {
                    IssueId = issue.Id,
                    Description = description,
                    CreatedDate = DateTime.Now
                };

                _unitOfWork.IssueHistory.Add(history);
            }
        }

        // API calls
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var issueList = _unitOfWork.Issue.GetAll();
            return Json(new { data = issueList });
        }

        [HttpGet]
        public IActionResult GetSpecificUserIssues(string userName)
        {
            var userId = _unitOfWork.User.GetFirstOrDefault(x => x.UserName == userName).Id;
            var userIssueAssignmentList = _unitOfWork.IssueAssignment.GetAll(x => x.UserId == userId);
            List<Issue> userIssueList = new List<Issue>();

            for(int i = 0; i < userIssueAssignmentList.Count(); i++)
            {
                userIssueList.Add(_unitOfWork.Issue.GetFirstOrDefault(x => x.Id == userIssueAssignmentList.ElementAt(i).IssueId));
            }

            return Json(new { data = userIssueList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDb = _unitOfWork.Issue.GetFirstOrDefault(x => x.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            // remove all history associated with this issue
            var historyFromDb = _unitOfWork.IssueHistory.GetAll(x => x.IssueId == id);
            _unitOfWork.IssueHistory.RemoveRange(historyFromDb);

            _unitOfWork.Issue.Remove(objFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Issue deleted succesfully.";
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
