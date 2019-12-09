using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.Project;
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
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfiguration.Configure().CreateMapper();
        }

        public async Task Create(CreateProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<CreateProjectDto, Project>(projectDto);
            await _unitOfWork.ProjectRepository.Create(projectEntity);
        }

        public IEnumerable<ProjectDto> GetAll()
        {
            var projectList = _unitOfWork.ProjectRepository.GetAll();
            var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);
            return projectDtoList;
        }

        public async Task<ProjectDto> GetById(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(projectId);

            var projectDto = _mapper.Map<Project, ProjectDto>(project);
            return projectDto;
        }
    }
}
