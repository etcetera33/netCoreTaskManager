using Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class CreateWorkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Progress { get; set; }
        public int WorkItemTypeId { get; set; }
        public WorkItemTypes WorkItemType
        {
            get
            {
                return ((WorkItemTypes)WorkItemTypeId);
            }
        }
        public int StatusId { get; set; }
        public int AssigneeId { get; set; }
        public int AuthorId { get; set; }
        public int ProjectId { get; set; }

        public UserDto Assignee { get; set; }

        public UserDto Author { get; set; }

        public ProjectDto Project { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<IFormFile> Files { get; set; }
    }
}
