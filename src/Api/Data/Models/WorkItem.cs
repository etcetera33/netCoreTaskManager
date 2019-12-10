using Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class WorkItem
    {
        public int WorkItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int WorkItemTypeId
        {
            get
            {
                return (int)WorkItemType;
            }
            set
            {
                WorkItemType = (WorkItemTypes)value;
            }
        }
        [EnumDataType(typeof(WorkItemTypes))]
        public WorkItemTypes WorkItemType { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [ForeignKey(nameof(User))]
        public int AssigneeId { get; set; }
        public User Assignee { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
