using IssueTracker.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICommentRepository Comment { get; private set; }
        public IIssueRepository Issue { get; private set; }
        public IUserRepository User { get; private set; }
        public IIssueAssignmentRepository IssueAssignment { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _dbContext = db;
            Comment = new CommentRepository(_dbContext);
            Issue = new IssueRepository(_dbContext);
            User = new UserRepository(_dbContext);
            IssueAssignment = new IssueAssignmentRepository(_dbContext);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
