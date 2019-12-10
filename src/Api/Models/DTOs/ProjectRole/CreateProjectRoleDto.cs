using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ProjectRole
{
    public class CreateProjectRoleDto
    {
        private DateTime _appliedAt;
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int RoleId { get; set; }
        public DateTime AppliedAt
        {
            get
            {
                return _appliedAt;
            }
            set
            {
                if (_appliedAt == null)
                    _appliedAt = value;
            }
        }
    }
}
