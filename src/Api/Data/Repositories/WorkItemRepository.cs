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
            return await _dbContext.WorkItems.Where(p => p.ProjectId == projectId).ToListAsync();
        }

        public async override Task<WorkItem> GetById(int id)
        {
            return await _dbContext.WorkItems
                .Include(x => x.Assignee)
                .FirstOrDefaultAsync(i => i.WorkItemId == id);
        }

        public async Task<IEnumerable<WorkItem>> GetAllByProjectNUserId(int projectId, int userId)
        {
            return await _dbContext.WorkItems
                .Where(x => x.ProjectId == projectId)
                .Where(x => x.AssigneeId == userId)
                .ToListAsync();
        }
    }
}
