﻿using Models.DTOs.WorkItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WorkItemDto>> GetAllAsync();
        Task Create(CreateWorkItemDto workItemDto);
        Task<WorkItemDto> GetById(int workItemId);
        Task Update(int workItemId, CreateWorkItemDto workItemDto);
        Task Remove(int workItemId);
        Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectId(int projectId);
        IEnumerable<WorkItemDto> GetAll();
    }
}
