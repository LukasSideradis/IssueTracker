using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.Repository
{
    public class IssueAssignmentRepository : Repository<IssueAssignment>, IIssueAssignmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public IssueAssignmentRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(IssueAssignment obj)
        {
            _dbContext.IssueAssignments.Update(obj);
        }
    }
}
