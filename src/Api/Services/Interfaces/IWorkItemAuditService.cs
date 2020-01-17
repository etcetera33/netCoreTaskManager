using Core.Enums;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWorkItemAuditService
    {
        Task<WorkItemAuditDto> Create(int workItemId, WIAuditStatuses status, WorkItemHistoryDto oldWorkItem = null, WorkItemHistoryDto newWorkItem = null);
        Task<IEnumerable<WorkItemAuditDto>> GetWorkItemsHistoryById(int workItemId);
    }
}
