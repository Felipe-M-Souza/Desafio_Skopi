using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [Required]
        public TaskPriority Priority { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int UserId { get; set; }
    }
    
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [Required]
        public Models.TaskStatus Status { get; set; }
        
        public DateTime? DueDate { get; set; }
    }
    
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Models.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<TaskHistoryDto> TaskHistories { get; set; } = new List<TaskHistoryDto>();
    }
    
    public class TaskHistoryDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
    
    public class AddTaskCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Comment { get; set; } = string.Empty;
        
        [Required]
        public int UserId { get; set; }
    }
}
