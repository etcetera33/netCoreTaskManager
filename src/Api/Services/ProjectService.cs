﻿using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.Project;
using Services.Interfaces;
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
    }
}
