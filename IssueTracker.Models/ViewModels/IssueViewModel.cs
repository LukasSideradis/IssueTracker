using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models.ViewModels
{
    /// <summary>
    /// Model for displaying issue details and accompanying tabs
    /// </summary> 
    public class IssueViewModel
    {
        // for all cases
        public int? IssueId { get; set; }
        public string? Parameter { get; set; }
        public string? UserName { get; set; }
        public Comment? Comment { get; set; }

        // for details
        public Issue? Issue { get; set; }

        // for comments
        public IEnumerable<Comment>? Comments { get; set; }

        // for history
        public IEnumerable<IssueHistory>? Histories { get; set; }

        // for assignments
        public AssignIssueViewModel? AssignIssueViewModel { get; set; }
    }
}
