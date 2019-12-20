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

        public async override Task<WorkItem> GetById(int id)
        {
            return await _dbContext.WorkItems
                .Include(x => x.Assignee)
                .FirstOrDefaultAsync(i => i.WorkItemId == id);
        }

        public async Task<IEnumerable<WorkItem>> PaginateFiltered(int projectId, int offset, int itemsCount, int assigneeId, string searchPhrase = "")
        {
            return await _dbContext.WorkItems
                .Where(x => x.ProjectId == projectId)
                .Where(x => x.Title.Contains(searchPhrase))
                .Where(x => x.AssigneeId == assigneeId)
                .Skip(offset)
                .Take(itemsCount)
                .ToListAsync();
        }

        public async Task<int> GetFilteredDataCountAsync(int projectId, int assigneeId, string searchPhrase)
        {
            return await _dbContext.WorkItems
                .Where(x => x.ProjectId == projectId)
                .Where(x => x.Title.Contains(searchPhrase))
                .Where(x => x.AssigneeId == assigneeId)
                .CountAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetTopFivePriorityItems(int assigneeId)
        {
            return await _dbContext.WorkItems
                .Where(x => x.AssigneeId == assigneeId)
                .OrderByDescending(x => x.Priority)
                .Take(5)
                .ToListAsync();
        }
    }
}
