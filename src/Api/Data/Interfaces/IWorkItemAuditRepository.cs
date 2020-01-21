using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IWorkItemAuditRepository : IBaseRepository<WorkItemAudit>
    {
        Task<IEnumerable<WorkItemAudit>> GetByWorkItemId(int workItemId);
    }
}
