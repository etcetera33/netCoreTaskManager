using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<IEnumerable<Project>> PaginateFiltered(int offset, int itemsCount, string searchPhrase = "");
        Task<int> GetFilteredDataCountAsync(string searchPhrase);
    }
}
