﻿using AutoMapper;
using Data.Interfaces;
using Data.Models;
using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IRedisService redisService, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _redisService = redisService;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Create(ProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<ProjectDto, Project>(projectDto);
            var createdEntity = await _projectRepository.Create(projectEntity);

            return _mapper.Map<Project, ProjectDto>(createdEntity);
        }

        public async Task<ProjectDto> GetById(int projectId)
        {
            if (projectId == 0)
            {
                return null;
            }

            var project = await _projectRepository.GetById(projectId);
            var projectDto = _mapper.Map<Project, ProjectDto>(project);

            return projectDto;
        }

        public async Task Update(int projectId, ProjectDto projectDto)
        {
            var project = _mapper.Map<ProjectDto, Project>(projectDto);
            await _projectRepository.Update(projectId, project);
        }

        public async Task<bool> ProjectExists(int projectId)
        {
            return (await _projectRepository.GetById(projectId) != null);
        }

        public async Task<BasePaginatedResponse<ProjectDto>> GetProjects(BaseQueryParameters parameters)
        {
            BasePaginatedResponse<ProjectDto> projectsPaginated = null;

            if (parameters.Search == "")
            {
                projectsPaginated = await _redisService.GetItemAsync<BasePaginatedResponse<ProjectDto>>($"projects.{parameters.ItemsPerPage}.{parameters.Page}");
            }

            if (projectsPaginated == null)
            {
                var projectList = await _projectRepository.PaginateFiltered(
                offset: (parameters.Page - 1) * parameters.ItemsPerPage,
                itemsCount: parameters.ItemsPerPage,
                searchPhrase: parameters.Search
                );

                var projectDtoList = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(projectList);
                var rowsCount = await _projectRepository.GetFilteredDataCountAsync(parameters.Search);

                var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

                projectsPaginated = new BasePaginatedResponse<ProjectDto>
                {
                    EntityList = projectDtoList,
                    PagesCount = pagesCount
                };

                if (parameters.Search == "")
                {
                    await _redisService.SetItemAsync($"projects.{parameters.ItemsPerPage}.{parameters.Page}", projectsPaginated, 60);
                }
            }

            return projectsPaginated;
        }
    }
}
