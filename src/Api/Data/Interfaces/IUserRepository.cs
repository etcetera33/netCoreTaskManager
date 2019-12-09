using Data.Models;

namespace Data.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        User FindUserByLoginPassword(string login, string password);
    }
}
