using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Project
{
    public class CreateProjectDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
    }
}
