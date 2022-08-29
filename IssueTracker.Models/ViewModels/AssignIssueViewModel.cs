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
    public class AssignIssueViewModel
    {
        public int? IssueId { get; set; }
        [ValidateNever]
        public Dictionary<string, bool> UserList { get; set; }
    }
}
