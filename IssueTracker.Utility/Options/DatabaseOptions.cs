using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Utility.Options
{
    public class DatabaseOptions
    {
        // Property names must match those in 'appsettings.json'

        public string ConnectionString { get; set; } = string.Empty;

        public int MaxRetryCount { get; set; }

        public int CommandTimeout { get; set; }

        public bool EnableDetailedErrors { get; set; }

        public bool EnableSensitiveDataLogging { get; set; }
    }
}
