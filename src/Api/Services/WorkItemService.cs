using AutoMapper;
using Core.Enums;
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
    public class WorkItemService : IWorkItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkItemService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = imapper;
        }

        public async Task<object> Paginate(int projectId, WorkItemQueryParameters parameters)
        {
            var wokrItemList = await _unitOfWork.WorkItemRepository.PaginateFiltered(
                projectId: projectId,
                offset: (parameters.Page - 1) * parameters.ItemsPerPage,
                itemsCount: parameters.ItemsPerPage,
                assigneeId: parameters.AssigneeId,
                searchPhrase: parameters.Search
                );

            var wokrItemDtoList = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(wokrItemList);

            var rowsCount = await _unitOfWork.WorkItemRepository.GetFilteredDataCountAsync(
                projectId: projectId,
                assigneeId: parameters.AssigneeId,
                searchPhrase: parameters.Search);

            var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

            return new
            {
                wokrItemList = wokrItemDtoList,
                pagesCount
            };
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

        public async Task Remove(int workItemId)
        {
            await _unitOfWork.WorkItemRepository.Delete(workItemId);
        }

        public async Task<IEnumerable<WorkItemDto>> GetTopFivePriorityItems(int assigneeId)
        {
            var workItems = await _unitOfWork.WorkItemRepository.GetTopFivePriorityItems(assigneeId);

            return _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItems);
        }

        public async Task Update(int workItemId, WorkItemDto workItemDto)
        {
            var workItem = _mapper.Map<WorkItemDto, WorkItem>(workItemDto);
            await _unitOfWork.WorkItemRepository.Update(workItemId, workItem);
        }

        public async Task<bool> WorkItemExists(int workItemId)
        {
            return (await _unitOfWork.WorkItemRepository.GetById(workItemId) != null);
        }

        public IEnumerable<object> GetWorkItemTypes()
        {
            var enumTypes = new List<object>();
            
            foreach (var item in WorkItemTypes.GetValues(typeof(WorkItemTypes)))
            {

                enumTypes.Add(new
                {
                    Id = (int)item,
                    Name = item.ToString()
                });
            }

            return enumTypes;
        }

        public IEnumerable<object> GetWorkItemStatuses()
        {
            var enumStatuses = new List<object>();

            foreach (var item in Statuses.GetValues(typeof(Statuses)))
            {

                enumStatuses.Add(new
                {
                    Id = (int)item,
                    Name = item.ToString()
                });
            }

            return enumStatuses;
        }
    }
}
