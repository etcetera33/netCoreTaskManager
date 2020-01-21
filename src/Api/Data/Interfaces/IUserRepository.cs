using Data.Models;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindUserByLoginAsync(string login);
        Task<bool> UserWithEmailExists(string email);
    }
}
