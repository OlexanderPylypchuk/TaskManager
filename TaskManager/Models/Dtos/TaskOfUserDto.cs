using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Dtos
{
    public class TaskOfUserDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public StatusTypes Status { get; set; }
        public PriorityTypes Priority { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ValidateNever]
        public User User { get; set; }
    }
}
