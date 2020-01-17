using Models.DTOs;

namespace Contracts
{
    public class WorkItemCreated
    {
        public int WorkItemId { get; set; }
        public WorkItemHistoryDto NewWorkItem { get; set; }
    }
}
