using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Utility
{
    public static class Globals
    {
        // Role strings
        public const string ROLE_USER_ADMIN = "Admin";              // can do everything
        public const string ROLE_USER_ADVANCED = "Advanced User";   // can assign users to issues, create issues
        public const string ROLE_USER_BASIC = "Basic User";         // can comment
        public const string ROLE_USER_GUEST = "Guest";              // can only look at issues

        // Issue priority strings
        public const string ISSUE_PRIORITY_CRITICAL = "Critical";
        public const string ISSUE_PRIORITY_IMPORTANT = "Important";
        public const string ISSUE_PRIORITY_STANDARD = "Standard";
        public const string ISSUE_PRIORITY_LOW = "Low";
        public const string ISSUE_PRIORITY_NOTASSIGNED = "Not assigned";

        // Issue status strings
        public const string ISSUE_STATUS_RESOLVED = "Resolved";
        public const string ISSUE_STATUS_ACTIVE = "Active";
        public const string ISSUE_STATUS_ONHOLD = "On hold";
        public const string ISSUE_STATUS_NEW = "New";

        // Issue type strings
        public const string ISSUE_TYPE_BUGS = "Bugs";
        public const string ISSUE_TYPE_VISUALS = "Visuals";
        public const string ISSUE_TYPE_OTHER = "Other";
    }
}
