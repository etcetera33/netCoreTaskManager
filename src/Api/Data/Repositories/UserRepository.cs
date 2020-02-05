using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<User> FindUserByLoginAsync(string login)
        {
            return await DbContext.Users
                //.Where(user => user.Login == login)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByExternalId(string externalId)
        {
            return await DbContext.Users
                .Where(user => user.ExternalId == externalId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserWithEmailExists(string email)
        {
            var user = await DbContext.Users
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();

            return user != null;
        }
    }
}
