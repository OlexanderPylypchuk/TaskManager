using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Dtos
{
    public class UserDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
