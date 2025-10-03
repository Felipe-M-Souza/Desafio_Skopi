using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        /// <summary>
        /// Lista todas as tarefas de um projeto
        /// </summary>
        [HttpGet("project/{projectId}/user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetProjectTasks(int projectId, int userId)
        {
            var tasks = await _taskService.GetProjectTasksAsync(projectId, userId);
            return Ok(tasks);
        }
        
        /// <summary>
        /// Obtém uma tarefa específica por ID
        /// </summary>
        [HttpGet("{taskId}/user/{userId}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int taskId, int userId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId, userId);
            
            if (task == null)
                return NotFound("Tarefa não encontrada");
                
            return Ok(task);
        }
        
        /// <summary>
        /// Cria uma nova tarefa (máximo 20 por projeto)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var task = await _taskService.CreateTaskAsync(createTaskDto);
                return CreatedAtAction(nameof(GetTask), 
                    new { taskId = task.Id, userId = task.UserId }, task);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar tarefa: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        [HttpPut("{taskId}/user/{userId}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(int taskId, int userId, UpdateTaskDto updateTaskDto)
        {
            try
            {
                var task = await _taskService.UpdateTaskAsync(taskId, updateTaskDto, userId);
                
                if (task == null)
                    return NotFound("Tarefa não encontrada");
                    
                return Ok(task);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar tarefa: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Remove uma tarefa
        /// </summary>
        [HttpDelete("{taskId}/user/{userId}")]
        public async Task<ActionResult> DeleteTask(int taskId, int userId)
        {
            try
            {
                var deleted = await _taskService.DeleteTaskAsync(taskId, userId);
                
                if (!deleted)
                    return NotFound("Tarefa não encontrada");
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir tarefa: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Adiciona um comentário a uma tarefa
        /// </summary>
        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult<TaskResponseDto>> AddTaskComment(int taskId, AddTaskCommentDto addCommentDto)
        {
            try
            {
                var task = await _taskService.AddTaskCommentAsync(taskId, addCommentDto);
                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar comentário: {ex.Message}");
            }
        }
    }
}
