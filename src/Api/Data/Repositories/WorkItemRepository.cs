using System;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return await DbContext.WorkItems
                .Include(x => x.Assignee)
                .FirstOrDefaultAsync(i => i.WorkItemId == id);
        }

        public async Task<IEnumerable<WorkItem>> PaginateFiltered(Expression<Func<WorkItem, bool>> expression, int offset, int itemsCount)
        {
            var res = DbContext.WorkItems
                .Where(expression)
                .Skip(offset)
                .Take(itemsCount);

            return await res.AsQueryable().ToListAsync();
        }

        public async Task<int> GetFilteredDataCountAsync(Expression<Func<WorkItem, bool>> expression)
        {
            return await DbContext.WorkItems
                .Where(expression)
                .CountAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetTopFivePriorityItems(int assigneeId)
        {
            return await DbContext.WorkItems
                .Where(x => x.AssigneeId == assigneeId)
                .OrderByDescending(x => x.Priority)
                .Take(5)
                .ToListAsync();
        }
    }
}
