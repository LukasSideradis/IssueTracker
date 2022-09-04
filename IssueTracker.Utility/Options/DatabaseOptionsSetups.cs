using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Utility.Options
{
    public class DatabaseOptionsSetups : IConfigureOptions<DatabaseOptions>
    {
        private const string ConfigurationSectionName = "DatabaseOptions";
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetups(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            options.ConnectionString = connectionString;

            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
