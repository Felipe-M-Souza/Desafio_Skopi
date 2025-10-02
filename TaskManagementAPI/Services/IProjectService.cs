using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetUserProjectsAsync(int userId);
        Task<ProjectResponseDto?> GetProjectByIdAsync(int projectId, int userId);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectResponseDto?> UpdateProjectAsync(int projectId, UpdateProjectDto updateProjectDto, int userId);
        Task<bool> DeleteProjectAsync(int projectId, int userId);
        Task<bool> CanDeleteProjectAsync(int projectId);
    }
}
