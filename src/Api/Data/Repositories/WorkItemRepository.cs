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
        public async Task<IEnumerable<WorkItem>> GetAll()
        {
            return await _dbContext.WorkItems
                .Include(x => x.Assignee)
                .Include(x => x.Author)
                .Include(x => x.Project)
                .Include(x => x.Comments)
                .ToListAsync();
        }
    }
}
