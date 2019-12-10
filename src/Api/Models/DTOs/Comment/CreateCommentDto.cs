using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public string CommentBody { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int WorkItemId { get; set; }
    }
}
