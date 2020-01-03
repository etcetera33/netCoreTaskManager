using Models.DTOs;

namespace Contracts
{
    public class WorkItemChanged
    {
        public string AssigneeFullName { get; set; }
        public string AssigneeEmail { get; set; }
        public int WorkItemId { get; set; }
        public EmailDto Email { get; set; }
    }
}
