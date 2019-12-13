using Models.DTOs.WorkItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WorkItemDto>> GetAll();
        Task<WorkItemDto> Create(WorkItemDto workItemDto);
        Task<WorkItemDto> GetById(int workItemId);
        Task Update(int workItemId, WorkItemDto workItemDto);
        Task Remove(int workItemId);
        Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectId(int projectId);
        Task<bool> WorkItemExists(int workItem);
    }
}
