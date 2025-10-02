using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Key
        public int UserId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
