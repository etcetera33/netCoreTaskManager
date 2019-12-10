using System.Collections.Generic;
using Models.DTOs.Project;
using System.Threading.Tasks;
using Models.DTOs.ProjectRole;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAll();
        Task Create(CreateProjectDto projectDto);
        Task<ProjectDto> GetById(int projectId);
        Task Update(int projectId, CreateProjectDto projectDto);
        Task GiveRole(CreateProjectRoleDto roleDto);
        Task<IEnumerable<ProjectRoleDto>> GetProjectRoles(int projectId);
        Task ChangeRole(int projectRoleId, ProjectRoleDto roleDto);
    }
}
