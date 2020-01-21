using Models.DTOs;

namespace Contracts
{
    public class WorkItemDeleted
    {
        public int WorkItemId { get; set; }
        public WorkItemHistoryDto OldWorkItem { get; set; }
    }
}
