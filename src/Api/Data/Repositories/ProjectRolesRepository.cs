using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProjectRolesRepository : BaseRepository<ProjectRole>
    {
        public ProjectRolesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<ProjectRole>> GetAllRolesByProjectId(int projectId)
        {
            return await _dbContext.ProjectRoles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
        }
    }
}
