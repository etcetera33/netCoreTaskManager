using Models.QueryParameters;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.PaginatedResponse;

namespace Services.Interfaces
{
    public interface IWorkItemService
    {
        Task<BasePaginatedResponse<WorkItemDto>> Paginate(int projectId, WorkItemQueryParameters parameters);
        Task<WorkItemDto> Create(WorkItemDto workItemDto);
        Task<WorkItemDto> GetById(int workItemId);
        Task Update(int workItemId, WorkItemDto workItemDto);
        Task Remove(int workItemId);
        Task<bool> WorkItemExists(int workItemId);
        IEnumerable<object> GetWorkItemTypes();
        IEnumerable<object> GetWorkItemStatuses();
        Task<IEnumerable<WorkItemDto>> GetTopFivePriorityItems(int assigneeId);
    }
}
