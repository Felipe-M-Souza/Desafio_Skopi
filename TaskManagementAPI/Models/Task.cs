using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
    
    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    
    public class Task
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public string Status { get; set; } = "Pending";
        
        public string Priority { get; set; } = "Medium";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        // Foreign Keys
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }
}
