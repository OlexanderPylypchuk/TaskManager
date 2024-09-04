using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TaskManager.Models
{
    public class TaskOfUser
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public StatusTypes Status { get; set; }
        public PriorityTypes Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public DateTime DueDate { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ValidateNever]
        public User User { get; set; }
    }
}
