using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.Project;
using Models.DTOs.ProjectRole;
using Services.Interfaces;
using Services.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectService: IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<CreateProjectDto, Project>(projectDto);
            await _unitOfWork.ProjectRepository.Create(projectEntity);
        }

        public async Task<IEnumerable<ProjectDto>> GetAll()
        {
            var projectList = await _unitOfWork.ProjectRepository.GetAll();
            var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);
            
            return projectDtoList;
        }

        public async Task<ProjectDto> GetById(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(projectId);
            var projectDto = _mapper.Map<Project, ProjectDto>(project);
            
            return projectDto;
        }

        public async Task Update(int projectId, CreateProjectDto projectDto)
        {
            var project = _mapper.Map<CreateProjectDto, Project>(projectDto);
            await _unitOfWork.ProjectRepository.Update(projectId, project);
        }

        public async Task GiveRole(CreateProjectRoleDto projectRoleDto)
        {
            var project = _mapper.Map<CreateProjectRoleDto, ProjectRole>(projectRoleDto);
            await _unitOfWork.ProjectRolesRepository.Create(project);
        }

        public async Task<IEnumerable<ProjectRoleDto>> GetProjectRoles(int projectId)
        {
            var roles = await _unitOfWork.ProjectRolesRepository.GetAllRolesByProjectId(projectId);
            var rolesDto = _mapper.Map<IEnumerable<ProjectRole>, IEnumerable<ProjectRoleDto>>(roles);

            return rolesDto;
        }

        public async Task ChangeRole(int projectRoleId, ProjectRoleDto roleDto)
        {
            var role = _mapper.Map<ProjectRoleDto, ProjectRole>(roleDto);
            await _unitOfWork.ProjectRolesRepository.Update(projectRoleId, role);
        }
    }
}
