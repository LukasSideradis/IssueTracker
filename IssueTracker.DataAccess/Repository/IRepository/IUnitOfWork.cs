using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICommentRepository Comment { get; }
        IIssueRepository Issue { get; }
        IUserRepository User { get; }
        IIssueAssignmentRepository IssueAssignment { get; }
        void Save();
    }
}
