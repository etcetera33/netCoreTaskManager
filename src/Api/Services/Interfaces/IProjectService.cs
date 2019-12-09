using System.Collections.Generic;
using Models.DTOs.Project;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectDto> GetAll();
        Task Create(CreateProjectDto projectDto);
        Task<ProjectDto> GetById(int projectId);
    }
}
