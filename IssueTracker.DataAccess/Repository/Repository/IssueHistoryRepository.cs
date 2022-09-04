using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.Repository
{
    public class IssueHistoryRepository : Repository<IssueHistory>, IIssueHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public IssueHistoryRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(IssueHistory obj)
        {
            _dbContext.IssueHistories.Update(obj);
        }
    }
}
