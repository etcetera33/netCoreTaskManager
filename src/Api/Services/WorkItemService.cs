using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.WorkItem;
using Services.Interfaces;
using Services.Mapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WorkItemService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = imapper;
        }
        public async Task<WorkItemDto> Create(CreateWorkItemDto workItemDto)
        {
            var workItemEntity = _mapper.Map<CreateWorkItemDto, WorkItem>(workItemDto); 
            var workItem = await _unitOfWork.WorkItemRepository.Create(workItemEntity);

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public Task<WorkItemDto> GetById(int workItemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectId(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task Remove(int workItemId)
        {
            throw new NotImplementedException();
        }

        public Task Update(int workItemId, CreateWorkItemDto workItemDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkItemDto>> GetAll()
        {
            var workItemList = await _unitOfWork.WorkItemRepository.GetAll();
            var workItemDto = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItemList);
            return workItemDto;
        }
    }
}
