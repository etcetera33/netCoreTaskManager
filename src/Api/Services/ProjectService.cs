using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectService: IProjectService
    {
        private const int ITEMS_PER_PAGE = 10;

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

        public async Task<ProjectDto> GetBySlug(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetById(projectId);

            return _mapper.Map<Project, ProjectDto>(project);
        }


        public async Task<IEnumerable<ProjectDto>> Paginate(int pageNumber)
        {
            var projectList = await _unitOfWork.ProjectRepository.Paginate(
                offset: ( pageNumber - 1 ) * ITEMS_PER_PAGE,
                itemsCount: ITEMS_PER_PAGE
                );

            var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);

            return projectDtoList;
        }

        public async Task<IEnumerable<ProjectDto>> Paginate(int pageNumber, string search)
        {
            var projectList = await _unitOfWork.ProjectRepository.PaginateFiltered(
                offset: (pageNumber - 1) * ITEMS_PER_PAGE,
                itemsCount: ITEMS_PER_PAGE,
                searchPhrase: search
                );
            
            var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);

            return projectDtoList;
        }

        public async Task<object> GetPaginatedDataAsync(int pageNumber, string search)
        {
            return new
            {
                projectList = await Paginate(pageNumber, search),
                pagesCount = CalculatePages(await _unitOfWork.ProjectRepository.GetFilteredDataCountAsync(search))
            };
        }

        public async Task<object> GetPaginatedDataAsync(int pageNumber)
        {
            return new
            {
                projectList = await Paginate(pageNumber),
                pagesCount = CalculatePages(await _unitOfWork.ProjectRepository.GetCountAsync())
            };
        }

        private int CalculatePages(int rowsCount)
        {
            return (int)Math.Ceiling((decimal)rowsCount / ITEMS_PER_PAGE);
        }
    }
}
