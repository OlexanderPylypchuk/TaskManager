using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace TaskManager.Models
{
    public class User
    {
        [Guid]
        [Key]
        public int Id { get; set; }
    }
}
