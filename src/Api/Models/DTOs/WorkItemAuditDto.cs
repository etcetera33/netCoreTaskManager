using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class WorkItemAuditDto
    {
        public int Id { get; set; }
        public int WorkItemId { get; set; }
        public WorkItemDto WorkItem { get; set; }
        public int StatusId
        {
            get
            {
                return (int)Status;
            }
            set
            {
                Status = (WorkItemAuditStatuses)value;
            }
        }
        [EnumDataType(typeof(WorkItemAuditStatuses))]
        public WorkItemAuditStatuses Status { get; set; }
        public string OldWorkItem { get; set; }
        public string NewWorkItem { get; set; }
    }
}
