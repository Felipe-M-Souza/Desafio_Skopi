using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TaskManagementDbContext _context;
        
        public ProjectService(TaskManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ProjectResponseDto>> GetUserProjectsAsync(int userId)
        {
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    TaskCount = p.Tasks.Count
                })
                .ToListAsync();
                
            return projects;
        }
        
        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .Where(p => p.Id == projectId && p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    TaskCount = p.Tasks.Count
                })
                .FirstOrDefaultAsync();
                
            return project;
        }
        
        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            var project = new Project
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                UserId = createProjectDto.UserId,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            
            var user = await _context.Users.FindAsync(createProjectDto.UserId);
            
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                UserId = project.UserId,
                UserName = user?.Name ?? string.Empty,
                TaskCount = 0
            };
        }
        
        public async Task<ProjectResponseDto?> UpdateProjectAsync(int projectId, UpdateProjectDto updateProjectDto, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
                
            if (project == null)
                return null;
                
            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;
            project.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            var user = await _context.Users.FindAsync(userId);
            
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                UserId = project.UserId,
                UserName = user?.Name ?? string.Empty,
                TaskCount = await _context.Tasks.CountAsync(t => t.ProjectId == projectId)
            };
        }
        
        public async Task<bool> DeleteProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
                
            if (project == null)
                return false;
                
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<bool> CanDeleteProjectAsync(int projectId)
        {
            var hasPendingTasks = await _context.Tasks
                .AnyAsync(t => t.ProjectId == projectId && t.Status != "Completed");
                
            return !hasPendingTasks;
        }
    }
}
