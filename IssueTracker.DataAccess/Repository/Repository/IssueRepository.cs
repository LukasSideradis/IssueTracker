using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.Repository
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public IssueRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(Issue obj)
        {
            _dbContext.Issues.Update(obj);
        }
    }
}
