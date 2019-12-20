using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProjectRepository: BaseRepository<Project>
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext) {}

        public async Task<IEnumerable<Project>> PaginateFiltered(int offset, int itemsCount, string searchPhrase = "")
        {
            return await _dbContext.Projects
                .Where(x => x.ProjectName.Contains(searchPhrase))
                .Skip(offset)
                .Take(itemsCount)
                .Include(x => x.Owner)
                .ToListAsync();
        }

        public async Task<int> GetFilteredDataCountAsync(string searchPhrase)
        {
            return await _dbContext.Projects
                .Where(x => x.ProjectName.Contains(searchPhrase))
                .CountAsync();
        }
    }
}
