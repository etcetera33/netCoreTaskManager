using System.Collections.Generic;

namespace Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        //public string PictureUrl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<WorkItem> AssignedTo { get; set; }
        public ICollection<WorkItem> CreatedWorkItems { get; set; }
    }
}
