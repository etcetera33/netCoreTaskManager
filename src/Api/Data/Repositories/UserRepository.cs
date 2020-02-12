using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<User> GetByExternalId(string externalId)
        {
            return await DbContext.Users
                .Where(user => user.ExternalId == externalId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetFilteredDataCountAsync(string search = "")
        {
            return await DbContext.Users
                .Where(x => x.FullName.Contains(search))
                .CountAsync();
        }

        public async Task<IEnumerable<User>> Paginate(int offset, int itemsCount, string search = "")
        {
            return await DbContext.Users
                .Where(x => x.FullName.Contains(search))
                .OrderBy(x => x.RoleId)
                .Skip(offset)
                .Take(itemsCount)
                .ToListAsync();
        }

        public async Task<bool> UserWithEmailExists(string email)
        {
            var user = await DbContext.Users
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();

            return user != null;
        }

        public async Task Patch(int id, dynamic changedData)
        {
            var entity = await DbContext.Users.FindAsync(id);

            DbContext.Entry(entity).CurrentValues.SetValues(changedData);
            await DbContext.SaveChangesAsync();
        }
    }
}
