using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task DeleteRange(IEnumerable<File> files)
        {
            DbContext.Files.RemoveRange(files);
            await DbContext.SaveChangesAsync();
        }
    }
}
