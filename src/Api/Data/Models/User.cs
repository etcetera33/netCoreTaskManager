using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int RoleId
        {
            get
            {
                return (int)Role;
            }
            set
            {
                Role = (Roles)value;
            }
        }
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<WorkItem> AssignedTo { get; set; }
        public ICollection<WorkItem> CreatedWorkItems { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
