using Core.Enums;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemAuditService
    {
        Task<WorkItemAuditDto> WICreated(int workItemId, WorkItemHistoryDto newWorkItem);
        Task<WorkItemAuditDto> WIUpdated(int workItemId, WorkItemHistoryDto oldWorkItem, WorkItemHistoryDto newWorkItem);
        Task<WorkItemAuditDto> WIDeleted(int workItemId, WorkItemHistoryDto oldWorkItem);
        Task<IEnumerable<WorkItemAuditDto>> GetWorkItemsHistoryById(int workItemId);
    }
}
