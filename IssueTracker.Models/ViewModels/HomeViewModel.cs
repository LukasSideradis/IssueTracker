using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models.ViewModels
{
    /// <summary>
    /// Model for displaying the varying dashboard elements on the home page
    /// </summary> 
    public class HomeViewModel
    {
        public IEnumerable<Issue>? Issues { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }

        // for selection by priority
        public Dictionary<string, int>? PriorityCount { get; set; }
    }
}
