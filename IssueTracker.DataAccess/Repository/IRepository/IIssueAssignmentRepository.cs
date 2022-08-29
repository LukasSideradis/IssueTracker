using IssueTracker.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repository.IRepository
{
    public interface IIssueAssignmentRepository : IRepository<IssueAssignment>
    {
        void Update(IssueAssignment obj);
    }
}
