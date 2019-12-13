using System.Collections.Generic;
using Models.DTOs.Project;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAll();
        Task<ProjectDto> Create(ProjectDto projectDto);
        Task<ProjectDto> GetById(int projectId);
        Task Update(int projectId, ProjectDto projectDto);
    }
}
