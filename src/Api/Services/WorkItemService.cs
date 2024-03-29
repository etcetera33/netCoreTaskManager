﻿using AutoMapper;
using Contracts;
using Core.Enums;
using Data.Interfaces;
using Data.Models;
using MassTransit;
using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly IFileUploader _fileUploader;
        private readonly IWorkItemFileRepository _workItemFileRepository;

        public WorkItemService(IWorkItemRepository workItemRepository, IMapper mapper, IBus bus, IFileUploader fileUploader, IWorkItemFileRepository workItemFileRepository)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
            _bus = bus;
            _fileUploader = fileUploader;
            _workItemFileRepository = workItemFileRepository;
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

            if (workItemDto.Files != null)
            {
                await AttachFilesToWorkItem(workItemDto.Files, workItemEntity.WorkItemId);
            }

            var newWorkItemDto = _mapper.Map<WorkItem, WorkItemHistoryDto>(workItem);
            await _bus.Publish(new WorkItemCreated
            {
                WorkItemId = workItem.WorkItemId,
                NewWorkItem = newWorkItemDto
            });

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task AttachFilesToWorkItem(IEnumerable<FileDto> files, int workItemId)
        {
            var entity = new List<WorkItemFile>();
            var filesId = files.Select(x => x.Id);

            Parallel.ForEach(files, file =>
            {
                entity.Add(new WorkItemFile { FileId = file.Id, WorkItemId = workItemId });
            });

            await _workItemFileRepository.AddRange(entity);
        }

        public async Task<WorkItemDto> GetById(int workItemId)
        {
            var workItem = await _workItemRepository.GetById(workItemId);

            return _mapper.Map<WorkItem, WorkItemDto>(workItem);
        }

        public async Task<IEnumerable<WorkItemDto>> GetTopFivePriorityItems(int assigneeId)
        {
            var workItems = await _workItemRepository.GetTopFivePriorityItems(assigneeId);

            return _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemDto>>(workItems);
        }

        public async Task Update(int workItemId, WorkItemDto workItemDto)
        {
            var oldWorkItem = await GetHistoryById(workItemId);

            var workItem = _mapper.Map<WorkItemDto, WorkItem>(workItemDto);

            await _workItemRepository.Update(workItemId, workItem);

            var newWorkItem = await GetHistoryById(workItemId);

            await AttachFilesToWorkItem(workItemDto.Files, workItemId);

            await _bus.Publish(new WorkItemUpdated
            {
                WorkItemId = workItemId,
                OldWorkItem = oldWorkItem,
                NewWorkItem = newWorkItem
            });
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

        public async Task<WorkItemHistoryDto> GetHistoryById(int workItemId)
        {
            var workItem = await _workItemRepository.GetByIdNoTracking(workItemId);

            return _mapper.Map<WorkItem, WorkItemHistoryDto>(workItem);
        }

        public async Task Delete(int workItemId)
        {
            var oldWorkItem = await GetHistoryById(workItemId);

            var filesToDelete = await GetAttachedById(workItemId);
            await _fileUploader.DeleteFromAzureAsync(filesToDelete);

            await _workItemRepository.Delete(workItemId);

            await _bus.Publish(new WorkItemDeleted
            {
                WorkItemId = workItemId,
                OldWorkItem = oldWorkItem
            });
        }

        public async Task<IEnumerable<FileDto>> GetAttachedById(int workItemId)
        {
            var entities = await _workItemFileRepository.GetByWorkItemId(workItemId);

            return _mapper.Map<IEnumerable<WorkItemFile>, IEnumerable<FileDto>>(entities);
        }
    }
}
