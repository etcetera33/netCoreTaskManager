using AutoMapper;
using Data;
using Data.Models;
using Models.QueryParameters;
using Models.DTOs;
using Services.Interfaces;
using System;
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

        public async Task<ProjectDto> Create(ProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<ProjectDto, Project>(projectDto);
            var createdEntity = await _unitOfWork.ProjectRepository.Create(projectEntity);
            return _mapper.Map<Project, ProjectDto>(createdEntity);
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

        public async Task Update(int projectId, ProjectDto projectDto)
        {
            var project = _mapper.Map<ProjectDto, Project>(projectDto);
            await _unitOfWork.ProjectRepository.Update(projectId, project);
        }

        public async Task<bool> ProjectExists(int projectId)
        {
            return (await _unitOfWork.ProjectRepository.GetById(projectId) != null);
        }

        public async Task<object> GetPaginatedDataAsync(BaseQueryParameters parameters)
        {
            var projectList = await _unitOfWork.ProjectRepository.PaginateFiltered(
                offset: (parameters.Page - 1) * parameters.ItemsPerPage,
                itemsCount: parameters.ItemsPerPage,
                searchPhrase: parameters.Search
                );

            var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);
            var rowsCount = await _unitOfWork.ProjectRepository.GetFilteredDataCountAsync(parameters.Search);

            var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

            return new
            {
                projectList = projectDtoList,
                pagesCount
            };
        }
    }
}
