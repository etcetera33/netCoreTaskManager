using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class ProjectRepository: BaseRepository<Project>
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext) {}
    }
}
