using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repositories
{
    public class CommentRepository: BaseRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public IEnumerable<Comment> GetCommentsByWorkItemId(int workItemId)
        {
            return _dbContext.Comments.Where(x => x.WorkItemId == workItemId).ToList();
        }
    }
}
