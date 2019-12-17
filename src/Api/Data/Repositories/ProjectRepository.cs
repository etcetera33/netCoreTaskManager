using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProjectRepository: BaseRepository<Project>
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext) {}
    }
}
