using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetProjectTasksAsync(int projectId, int userId);
        Task<TaskResponseDto?> GetTaskByIdAsync(int taskId, int userId);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskResponseDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId);
        Task<bool> DeleteTaskAsync(int taskId, int userId);
        Task<TaskResponseDto> AddTaskCommentAsync(int taskId, AddTaskCommentDto addCommentDto);
        Task<bool> CanCreateTaskAsync(int projectId);
    }
}
