using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs.ProjectRole
{
    public class ProjectRoleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
