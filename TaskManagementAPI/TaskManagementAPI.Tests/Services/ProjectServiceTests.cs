using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Services;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Tests.Services;

public class ProjectServiceTests : IDisposable
{
    private readonly TaskManagementDbContext _context;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementDbContext(options);
        _projectService = new ProjectService(_context);
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldCreateProject_WhenValidData()
    {
        // Arrange
        var createProjectDto = new CreateProjectDto
        {
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1
        };

        // Act
        var result = await _projectService.CreateProjectAsync(createProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Project", result.Name);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task GetUserProjectsAsync_ShouldReturnProjects_WhenUserHasProjects()
    {
        // Arrange
        var project1 = new Project
        {
            Id = 1,
            Name = "Project 1",
            Description = "Description 1",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        var project2 = new Project
        {
            Id = 2,
            Name = "Project 2",
            Description = "Description 2",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.AddRange(project1, project2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.GetUserProjectsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Project 1");
        Assert.Contains(result, p => p.Name == "Project 2");
    }

    [Fact]
    public async Task GetUserProjectsAsync_ShouldReturnEmptyList_WhenUserHasNoProjects()
    {
        // Act
        var result = await _projectService.GetUserProjectsAsync(999);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CanDeleteProjectAsync_ShouldReturnTrue_WhenProjectHasNoPendingTasks()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        var completedTask = new Models.Task
        {
            Id = 1,
            Title = "Completed Task",
            Description = "Description",
            Status = "Completed",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(completedTask);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.CanDeleteProjectAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanDeleteProjectAsync_ShouldReturnFalse_WhenProjectHasPendingTasks()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        var pendingTask = new Models.Task
        {
            Id = 1,
            Title = "Pending Task",
            Description = "Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(pendingTask);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.CanDeleteProjectAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldDeleteProject_WhenProjectCanBeDeleted()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.DeleteProjectAsync(1, 1);

        // Assert
        Assert.True(result);
        Assert.False(_context.Projects.Any(p => p.Id == 1));
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectNotFound()
    {
        // Act
        var result = await _projectService.DeleteProjectAsync(999, 1);

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
