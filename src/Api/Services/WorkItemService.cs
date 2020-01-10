using AutoMapper;
using Core.Enums;
using Data.Models;
using Models.QueryParameters;
using Models.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Services.Helpers;
using MassTransit;
using Contracts;
using Data.Interfaces;
using Models.PaginatedResponse;

namespace Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public WorkItemService(IWorkItemRepository workItemRepository, IMapper mapper, IBus bus)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<BasePaginatedResponse<WorkItemDto>> Paginate(int projectId, WorkItemQueryParameters parameters)
        {
            Expression<Func<WorkItem, bool>> exp = w => w.ProjectId == projectId;

            if (parameters.AssigneeId.HasValue)
            {
                exp = exp.AndAlso(w => w.AssigneeId == parameters.AssigneeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                exp = exp.AndAlso(w => w.Title.Contains(parameters.Search));
            }

            var workItemList = await _workItemRepository.PaginateFiltered(
                exp,
                offset: (parameters.Page - 1) * parameters.ItemsPerPage,
                itemsCount: parameters.ItemsPerPage
            );

            var workItemDtoList = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItemList);

            var rowsCount = await _workItemRepository.GetFilteredDataCountAsync(exp);

            var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

            return new BasePaginatedResponse<WorkItemDto>
            {
                EntityList = workItemDtoList,
                PagesCount = pagesCount
            };
        }

        public async Task<WorkItemDto> Create(WorkItemDto workItemDto)
        {
            var workItemEntity = _mapper.Map<WorkItemDto, WorkItem>(workItemDto); 
            var workItem = await _workItemRepository.Create(workItemEntity);

            await _bus.Publish(new WorkItemChanged {
                WorkItemId = workItem.WorkItemId
            });

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task<WorkItemDto> GetById(int workItemId)
        {
            var workItem = await _workItemRepository.GetById(workItemId);

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task Remove(int workItemId)
        {
            await _workItemRepository.Delete(workItemId);
        }

        public async Task<IEnumerable<WorkItemDto>> GetTopFivePriorityItems(int assigneeId)
        {
            var workItems = await _workItemRepository.GetTopFivePriorityItems(assigneeId);

            return _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItems);
        }

        public async Task Update(int workItemId, WorkItemDto workItemDto)
        {
            var workItem = _mapper.Map<WorkItemDto, WorkItem>(workItemDto);

            await _workItemRepository.Update(workItemId, workItem);

            if (await IsAssigneeChanged(workItemId, workItemDto.AssigneeId))
            {
                await _bus.Publish(new WorkItemChanged
                {
                    WorkItemId = workItem.WorkItemId
                });
            }
        }

        public async Task<bool> WorkItemExists(int workItemId)
        {
            return (await _workItemRepository.GetById(workItemId) != null);
        }

        public IEnumerable<object> GetWorkItemTypes()
        {
            var enumTypes = new List<object>();
            
            foreach (var item in Enum.GetValues(typeof(WorkItemTypes)))
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

            foreach (var item in Enum.GetValues(typeof(Statuses)))
            {

                enumStatuses.Add(new
                {
                    Id = (int)item,
                    Name = item.ToString()
                });
            }

            return enumStatuses;
        }

        private async Task<bool> IsAssigneeChanged(int workItemId, int assigneeId)
        {
            var workItem = await _workItemRepository.GetById(workItemId);

            return workItem.AssigneeId != assigneeId;
        }
    }
}
