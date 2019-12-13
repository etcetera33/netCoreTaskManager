using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class WorkItemRepository : BaseRepository<WorkItem>
    {
        public WorkItemRepository(ApplicationDbContext dbContext) : base (dbContext)
        {

        }

        public async Task<IEnumerable<WorkItem>> GetAllByProjectId(int projectId)
        {
            return await _dbContext.WorkItems.Where(x => x.ProjectId == projectId).ToListAsync();
        }
    }
}
