using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<File>> Paginate(int offset, int itemsCount)
        {
            return await DbContext.Files
                .OrderBy(x => x.FileId)
                .Skip(offset)
                .Take(itemsCount)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbContext.Files
                .CountAsync();
        }
    }
}
