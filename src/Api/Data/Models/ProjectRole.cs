using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class ProjectRole
    {
        public int ProjectRoleId { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime AppliedAt { get; set; }
    }
}
