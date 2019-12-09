using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentBody { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        [ForeignKey(nameof(WorkItem))]
        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
