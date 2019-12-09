using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
