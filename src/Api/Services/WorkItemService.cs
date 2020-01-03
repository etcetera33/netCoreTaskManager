using AutoMapper;
using Core.Enums;
using Data;
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
using Core.Configs;
using Microsoft.Extensions.Options;

namespace Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private IUserService _userService;
        private readonly IOptions<MailConfig> _mailConfig;

        public WorkItemService(IUnitOfWork unitOfWork, IMapper mapper, IBus bus, IUserService userService, IOptions<MailConfig> mailConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bus = bus;
            _userService = userService;
            _mailConfig = mailConfig;
        }

        public async Task<object> Paginate(int projectId, WorkItemQueryParameters parameters)
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

            var workItemList = await _unitOfWork.WorkItemRepository.PaginateFiltered(
                exp,
                offset: (parameters.Page - 1) * parameters.ItemsPerPage,
                itemsCount: parameters.ItemsPerPage
            );

            var workItemDtoList = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItemList);

            var rowsCount = await _unitOfWork.WorkItemRepository.GetFilteredDataCountAsync(exp);

            var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

            return new
            {
                wokrItemList = workItemDtoList,
                pagesCount
            };
        }

        public async Task<WorkItemDto> Create(WorkItemDto workItemDto)
        {
            var workItemEntity = _mapper.Map<WorkItemDto, WorkItem>(workItemDto); 
            var workItem = await _unitOfWork.WorkItemRepository.Create(workItemEntity);

            var user = await _userService.GetById(workItem.AssigneeId);
            var email = _mapper.Map<MailConfig, EmailDto>(_mailConfig.Value);

            await _bus.Publish(new WorkItemChanged {
                AssigneeFullName = user.FullName,
                AssigneeEmail = user.Email,
                WorkItemId = workItem.WorkItemId,
                Email = email
            });

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
    }
}
