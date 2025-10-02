using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskManagementDbContext _context;
        
        public TaskService(TaskManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<TaskResponseDto>> GetProjectTasksAsync(int projectId, int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.UserId == userId)
                .Include(t => t.Project)
                .Include(t => t.User)
                .Include(t => t.TaskHistories)
                    .ThenInclude(th => th.User)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    DueDate = t.DueDate,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name,
                    UserId = t.UserId,
                    UserName = t.User.Name,
                    TaskHistories = t.TaskHistories.Select(th => new TaskHistoryDto
                    {
                        Id = th.Id,
                        Comment = th.Comment,
                        CreatedAt = th.CreatedAt,
                        UserName = th.User.Name
                    }).OrderBy(th => th.CreatedAt).ToList()
                })
                .ToListAsync();
                
            return tasks;
        }
        
        public async Task<TaskResponseDto?> GetTaskByIdAsync(int taskId, int userId)
        {
            var task = await _context.Tasks
                .Where(t => t.Id == taskId && t.UserId == userId)
                .Include(t => t.Project)
                .Include(t => t.User)
                .Include(t => t.TaskHistories)
                    .ThenInclude(th => th.User)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    DueDate = t.DueDate,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name,
                    UserId = t.UserId,
                    UserName = t.User.Name,
                    TaskHistories = t.TaskHistories.Select(th => new TaskHistoryDto
                    {
                        Id = th.Id,
                        Comment = th.Comment,
                        CreatedAt = th.CreatedAt,
                        UserName = th.User.Name
                    }).OrderBy(th => th.CreatedAt).ToList()
                })
                .FirstOrDefaultAsync();
                
            return task;
        }
        
        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = new Models.Task
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Priority = createTaskDto.Priority,
                DueDate = createTaskDto.DueDate,
                ProjectId = createTaskDto.ProjectId,
                UserId = createTaskDto.UserId,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            // Add initial comment to history
            var initialComment = new TaskHistory
            {
                Comment = $"Tarefa criada com prioridade {createTaskDto.Priority}",
                TaskId = task.Id,
                UserId = createTaskDto.UserId,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.TaskHistories.Add(initialComment);
            await _context.SaveChangesAsync();
            
            return await GetTaskByIdAsync(task.Id, createTaskDto.UserId) ?? new TaskResponseDto();
        }
        
        public async Task<TaskResponseDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
                
            if (task == null)
                return null;
                
            var oldStatus = task.Status;
            var oldTitle = task.Title;
            var oldDescription = task.Description;
            
            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Status = updateTaskDto.Status;
            task.DueDate = updateTaskDto.DueDate;
            task.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            // Add update comment to history
            var changes = new List<string>();
            if (oldTitle != updateTaskDto.Title)
                changes.Add($"Título alterado de '{oldTitle}' para '{updateTaskDto.Title}'");
            if (oldDescription != updateTaskDto.Description)
                changes.Add("Descrição atualizada");
            if (oldStatus != updateTaskDto.Status)
                changes.Add($"Status alterado de {oldStatus} para {updateTaskDto.Status}");
                
            if (changes.Any())
            {
                var updateComment = new TaskHistory
                {
                    Comment = $"Tarefa atualizada: {string.Join(", ", changes)}",
                    TaskId = taskId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                
                _context.TaskHistories.Add(updateComment);
                await _context.SaveChangesAsync();
            }
            
            return await GetTaskByIdAsync(taskId, userId);
        }
        
        public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
                
            if (task == null)
                return false;
                
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<TaskResponseDto> AddTaskCommentAsync(int taskId, AddTaskCommentDto addCommentDto)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId);
                
            if (task == null)
                throw new ArgumentException("Tarefa não encontrada");
                
            var taskHistory = new TaskHistory
            {
                Comment = addCommentDto.Comment,
                TaskId = taskId,
                UserId = addCommentDto.UserId,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.TaskHistories.Add(taskHistory);
            await _context.SaveChangesAsync();
            
            return await GetTaskByIdAsync(taskId, addCommentDto.UserId) ?? new TaskResponseDto();
        }
        
        public async Task<bool> CanCreateTaskAsync(int projectId)
        {
            var taskCount = await _context.Tasks.CountAsync(t => t.ProjectId == projectId);
            return taskCount < 20;
        }
    }
}
