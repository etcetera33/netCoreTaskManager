using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFileRepository : IBaseRepository<File>
    {
        Task<IEnumerable<File>> Paginate(int offset, int itemsCount);
        Task<int> CountAsync();
    }
}
