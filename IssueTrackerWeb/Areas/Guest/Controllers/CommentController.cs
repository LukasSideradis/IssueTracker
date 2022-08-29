using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IssueTrackerWeb.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public IssueViewModel IssueViewModel { get; set; }

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IssueViewModel obj)
        {
            var userid = _unitOfWork.User.GetFirstOrDefault(x => x.UserName == obj.UserName).Id;
            obj.Comment.UserId = userid;
            obj.Comment.IssueId = (int)obj.IssueId;

            ModelState.ClearValidationState("Comment.UserId");
            if (TryValidateModel(obj))
            {
				// add non-inputable properties
				obj.Comment.CreatedDate = DateTime.Now;
                obj.Comment.LastUpdated = DateTime.Now;

				_unitOfWork.Comment.Add(obj.Comment);
				_unitOfWork.Save();
                TempData["success"] = "Comment posted successfully.";
                return RedirectToAction("Details", controllerName: "Issue", new { id = obj.IssueId, param = obj.Parameter } );
            }
            return RedirectToAction("Details", controllerName: "Issue", new { id = obj.IssueId, param = obj.Parameter } );
        }
    }
}
