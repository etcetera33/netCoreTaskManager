using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> UserWithEmailExists(string email);
        Task<User> GetByExternalId(string externalId);
        Task<IEnumerable<User>> Paginate(int offset, int itemsCount, string search = "");
        Task<int> GetFilteredDataCountAsync(string search="");
        Task Patch(int id, dynamic changedData);
    }
}
