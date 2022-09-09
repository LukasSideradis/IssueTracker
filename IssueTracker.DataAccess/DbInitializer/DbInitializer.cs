using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using IssueTracker.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            // roles if they are not created
            if (!_roleManager.RoleExistsAsync(Globals.ROLE_USER_ADMIN).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Globals.ROLE_USER_ADMIN)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Globals.ROLE_USER_ADVANCED)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Globals.ROLE_USER_BASIC)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    UserName = "admin@issuetracker",
                    Email = "yajopeb121@offsala.com",
                }, "7EuQTsJt8Dbyw4q+").GetAwaiter().GetResult();

                // if roles are not created, then create admin user
                User user = _db.Users.FirstOrDefault(x => x.UserName == "admin@issuetracker");  // could use UnitOfWork too

                _userManager.AddToRoleAsync(user, Globals.ROLE_USER_ADMIN).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
