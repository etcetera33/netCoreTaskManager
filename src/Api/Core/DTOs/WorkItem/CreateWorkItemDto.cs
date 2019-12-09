using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.WorkItem
{
    public class CreateWorkItemDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int WorkItemTypeId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int AssigneeId { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
