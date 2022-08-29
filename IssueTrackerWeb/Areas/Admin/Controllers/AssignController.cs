using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Models.ViewModels;
using IssueTracker.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTrackerWeb.Areas.Guest.Controllers
{
    [Area("Admin")]
    public class AssignController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public AssignRoleViewModel AssignRoleViewModel { get; set; }

        public AssignController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            AssignRoleViewModel = new AssignRoleViewModel()
            {
                UserList = _unitOfWork.User.GetAll().Select(x => new SelectListItem
                {
                    Text = x.UserName,
                    Value = x.Id.ToString()
                }),
                RoleList = new List<SelectListItem>()
            };

            // populate the role list manually
            AssignRoleViewModel.RoleList.Add(new SelectListItem
            {
                Text = Globals.ROLE_USER_ADMIN,
                Value = Globals.ROLE_USER_ADMIN
            });
            AssignRoleViewModel.RoleList.Add(new SelectListItem
            {
                Text = Globals.ROLE_USER_ADVANCED,
                Value = Globals.ROLE_USER_ADVANCED
            });
            AssignRoleViewModel.RoleList.Add(new SelectListItem
            {
                Text = Globals.ROLE_USER_BASIC,
                Value = Globals.ROLE_USER_BASIC
            });

            return View(AssignRoleViewModel);
        }

        public async Task<IActionResult> UpdateRole(AssignRoleViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(obj.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var currentRole = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole.ElementAt(0));
                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddToRoleAsync(user, obj.Role);
                    if (addResult.Succeeded)
                    {
                        TempData["success"] = "Changes applied successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "An error occured while applying changes.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "An error occured while applying changes.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["error"] = "An error occured while applying changes.";
                return RedirectToAction("Index");
            }
        }
    }
}
