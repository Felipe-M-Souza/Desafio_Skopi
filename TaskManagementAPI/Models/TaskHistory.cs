using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskHistory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Comment { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign Keys
        public int TaskId { get; set; }
        public int UserId { get; set; }
        
        // Navigation properties
        public virtual Task Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
