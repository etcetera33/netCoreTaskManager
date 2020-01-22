using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemAuditService
    {
        Task<WorkItemAuditDto> LogWorkItemCreation(int workItemId, WorkItemHistoryDto newWorkItem);
        Task<WorkItemAuditDto> LogWorkItemEditing(int workItemId, WorkItemHistoryDto oldWorkItem, WorkItemHistoryDto newWorkItem);
        Task<WorkItemAuditDto> LogWorkItemDeletion(int workItemId, WorkItemHistoryDto oldWorkItem);
        Task<IEnumerable<WorkItemAuditDto>> GetWorkItemsHistoryById(int workItemId);
    }
}
