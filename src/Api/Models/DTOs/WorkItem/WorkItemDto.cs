using Core.Enums;
using Models.DTOs.Comment;
using Models.DTOs.Project;
using System.Collections.Generic;

namespace Models.DTOs.WorkItem
{
    public class WorkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int WorkItemTypeId { get; set; }
        public WorkItemTypes WorkItemType { get; set; }
        public int StatusId { get; set; }
        public int AssigneeId { get; set; }
        public int AuthorId { get; set; }
        public int ProjectId { get; set; }

        public UserDto Assignee { get; set; }

        public UserDto Author { get; set; }

        public ProjectDto Project { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
