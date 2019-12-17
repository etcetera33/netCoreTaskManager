using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository: BaseRepository<User>
    {
        public UserRepository(ApplicationDbContext dbContext): base(dbContext) {}

        public async Task<User> FindUserByLoginAsync(string login)
        {
            return await _dbContext.Users
                .Where(user => user.Login == login)
                .FirstOrDefaultAsync();
        }
    }
}
