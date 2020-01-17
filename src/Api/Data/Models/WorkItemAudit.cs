using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class WorkItemAudit
    {
        public int WorkItemAuditId { get; set; }
        public int WorkItemId { get; set; }
        public WIAuditStatuses Status { get; set; }
        public string OldWorkItem { get; set; }
        public string NewWorkItem { get; set; }
    }
}
