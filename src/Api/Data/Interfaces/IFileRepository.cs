using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFileRepository : IBaseRepository<File>
    {
        Task DeleteRange(IEnumerable<File> files);
    }
}
