using Models.DTOs;

namespace Contracts
{
    public class WorkItemUpdated
    {
        public int WorkItemId { get; set; }
        public WorkItemHistoryDto OldWorkItem { get; set; }
        public WorkItemHistoryDto NewWorkItem { get; set; }
    }
}
