using System.Collections.Generic;
using Models.DTOs;
using System.Threading.Tasks;
using Models.QueryParameters;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAll();
        Task<ProjectDto> Create(ProjectDto projectDto);
        Task<ProjectDto> GetById(int projectId);
        Task Update(int projectId, ProjectDto projectDto);
        Task<bool> ProjectExists(int projectId);
        Task<object> GetPaginatedDataAsync(BaseQueryParameters parameters);
    }
}
