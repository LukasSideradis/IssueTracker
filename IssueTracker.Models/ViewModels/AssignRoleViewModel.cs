using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UserList { get; set; }
        [ValidateNever]
        public List<SelectListItem> RoleList { get; set; }
    }
}
