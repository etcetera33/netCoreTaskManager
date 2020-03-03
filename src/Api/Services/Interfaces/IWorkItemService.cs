using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemService
    {
        Task<BasePaginatedResponse<WorkItemDto>> Paginate(int projectId, WorkItemQueryParameters parameters);
        Task<WorkItemDto> Create(WorkItemDto workItemDto);
        Task<WorkItemDto> GetById(int workItemId);
        Task Update(int workItemId, WorkItemDto workItemDto);
        Task<bool> WorkItemExists(int workItemId);
        IEnumerable<object> GetWorkItemTypes();
        IEnumerable<object> GetWorkItemStatuses();
        Task<IEnumerable<WorkItemDto>> GetTopFivePriorityItems(int assigneeId);
        Task<IEnumerable<FileDto>> GetAttachedById(int workItemId);
        Task Delete(int workItemId);
    }
}
