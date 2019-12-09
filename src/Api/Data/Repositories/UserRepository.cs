using Data.Models;
using System.Linq;

namespace Data.Repositories
{
    public class UserRepository: BaseRepository<User>
    {
        public UserRepository(ApplicationDbContext dbContext): base(dbContext) {}

        public User FindUserByLoginPassword(string login, string password)
        {
            return _dbContext.Users
                .Where(user => user.Login == login)
                .Where(user => user.Password == password)
                .FirstOrDefault();
        }
    }
}
