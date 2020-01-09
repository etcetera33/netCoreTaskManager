using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository: BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Comment>> GetCommentsByWorkItemIdAsync(int workItemId)
        {
            return await DbContext.Comments
                .Where(x => x.WorkItemId == workItemId)
                .Include(x => x.Author)
                .OrderByDescending(x => x.CommentId)
                .ToListAsync();
        }
    }
}
