using System;

namespace Models.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime SentAt { get; set; }
        public int AuthorId { get; set; }
        public UserDto Author { get; set; }
        public int WorkItemId { get; set; }
    }
}
