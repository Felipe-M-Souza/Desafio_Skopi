using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        
        /// <summary>
        /// Lista todos os projetos de um usuário
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetUserProjects(int userId)
        {
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }
        
        /// <summary>
        /// Obtém um projeto específico por ID
        /// </summary>
        [HttpGet("{projectId}/user/{userId}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int projectId, int userId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId, userId);
            
            if (project == null)
                return NotFound("Projeto não encontrado");
                
            return Ok(project);
        }
        
        /// <summary>
        /// Cria um novo projeto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject(CreateProjectDto createProjectDto)
        {
            try
            {
                var project = await _projectService.CreateProjectAsync(createProjectDto);
                return CreatedAtAction(nameof(GetProject), 
                    new { projectId = project.Id, userId = project.UserId }, project);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar projeto: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Atualiza um projeto existente
        /// </summary>
        [HttpPut("{projectId}/user/{userId}")]
        public async Task<ActionResult<ProjectResponseDto>> UpdateProject(int projectId, int userId, UpdateProjectDto updateProjectDto)
        {
            try
            {
                var project = await _projectService.UpdateProjectAsync(projectId, updateProjectDto, userId);
                
                if (project == null)
                    return NotFound("Projeto não encontrado");
                    
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar projeto: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Remove um projeto (apenas se não houver tarefas pendentes)
        /// </summary>
        [HttpDelete("{projectId}/user/{userId}")]
        public async Task<ActionResult> DeleteProject(int projectId, int userId)
        {
            try
            {
                // Check if project can be deleted (no pending tasks)
                var canDelete = await _projectService.CanDeleteProjectAsync(projectId);
                if (!canDelete)
                    return BadRequest("Não é possível excluir o projeto pois existem tarefas pendentes");
                
                var deleted = await _projectService.DeleteProjectAsync(projectId, userId);
                
                if (!deleted)
                    return NotFound("Projeto não encontrado");
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir projeto: {ex.Message}");
            }
        }
    }
}
