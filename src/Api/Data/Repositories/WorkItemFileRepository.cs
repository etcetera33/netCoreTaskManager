using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class WorkItemFileRepository : BaseRepository<WorkItemFile>, IWorkItemFileRepository
    {
        public WorkItemFileRepository(ApplicationDbContext context) : base (context)
        {

        }

        public async Task AddRange(IEnumerable<WorkItemFile> entityList)
        {
            DbContext.WorkItemFiles.AddRange(entityList);
            await DbContext.SaveChangesAsync();
        }

        public async Task<WorkItemFile> GetByFileNWorkItemId(int fileId, int workItemId)
        {
            return await DbContext.WorkItemFiles
                .Where(x => x.FileId == fileId)
                .Where(x => x.WorkItemId == workItemId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<WorkItemFile>> GetByWorkItemId(int workItemId)
        {
            return await DbContext.WorkItemFiles
                .Where(x => x.WorkItemId == workItemId)
                .Include(x => x.File)
                .ToListAsync();
        }
    }
}
