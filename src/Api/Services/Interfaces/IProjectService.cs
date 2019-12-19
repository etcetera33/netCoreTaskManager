using System.Collections.Generic;
using Models.DTOs;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAll();
        Task<ProjectDto> Create(ProjectDto projectDto);
        Task<ProjectDto> GetById(int projectId);
        Task Update(int projectId, ProjectDto projectDto);
        Task<bool> ProjectExists(int projectId);
        Task<object> GetPaginatedDataAsync(int pageNumber);
        Task<object> GetPaginatedDataAsync(int pageNumber, string searchPhrase);
        Task<IEnumerable<ProjectDto>> Paginate(int pageNumber);
        Task<IEnumerable<ProjectDto>> Paginate(int pageNumber, string searchPhrase);
    }
}
