using System;

namespace Models.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime SentAt { get; set; }
        public int AuthorId { get; set; }
        public int WorkItemId { get; set; }
    }
}
