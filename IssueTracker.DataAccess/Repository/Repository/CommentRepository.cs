using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(Comment obj)
        {
            _dbContext.Comments.Update(obj);
        }
    }
}
