using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class WorkItemRepository : BaseRepository<WorkItem>
    {
        public WorkItemRepository(ApplicationDbContext dbContext) : base (dbContext)
        {

        }
        public IEnumerable<WorkItem> GetAll()
        {
            return _dbContext.WorkItems
                .Include(x => x.Assignee)
                .Include(x => x.Author)
                .Include(x => x.Project)
                .Include(x => x.Comments)
                .ToList();
        }
    }
}
