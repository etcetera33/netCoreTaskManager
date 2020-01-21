using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class WorkItemAuditRepository : BaseRepository<WorkItemAudit>, IWorkItemAuditRepository
    {
        public WorkItemAuditRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<WorkItemAudit>> GetByWorkItemId(int workItemId)
        {
            return await DbContext.WorkItemAudits
                .Where(x => x.WorkItemId == workItemId)
                .ToListAsync();
        }
    }
}
