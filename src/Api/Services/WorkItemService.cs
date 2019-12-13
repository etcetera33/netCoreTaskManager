using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.WorkItem;
using Services.Interfaces;
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

        public async Task<WorkItemDto> Create(WorkItemDto workItemDto)
        {
            var workItemEntity = _mapper.Map<WorkItemDto, WorkItem>(workItemDto); 
            var workItem = await _unitOfWork.WorkItemRepository.Create(workItemEntity);

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task<WorkItemDto> GetById(int workItemId)
        {
            var workItem = await _unitOfWork.WorkItemRepository.GetById(workItemId);

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectId(int projectId)
        {
            var workItems = await _unitOfWork.WorkItemRepository.GetAllByProjectId(projectId);

            return _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItems);
        }

        public async Task Remove(int workItemId)
        {
            await _unitOfWork.WorkItemRepository.Delete(workItemId);
        }

        public async Task Update(int workItemId, WorkItemDto workItemDto)
        {
            var workItem = _mapper.Map<WorkItemDto, WorkItem>(workItemDto);
            await _unitOfWork.WorkItemRepository.Update(workItemId, workItem);
        }

        public async Task<IEnumerable<WorkItemDto>> GetAll()
        {
            var workItemList = await _unitOfWork.WorkItemRepository.GetAll();
            var workItemDto = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItemList);

            return workItemDto;
        }

        public async Task<bool> WorkItemExists(int workItemId)
        {
            return (await _unitOfWork.WorkItemRepository.GetById(workItemId) != null);
        }
    }
}
