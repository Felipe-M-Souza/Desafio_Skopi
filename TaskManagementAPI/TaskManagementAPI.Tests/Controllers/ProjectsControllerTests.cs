using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Controllers;

public class ProjectsControllerTests : IDisposable
{
    private readonly TaskManagementDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ProjectsController _controller;

    public ProjectsControllerTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementDbContext(options);
        _projectService = new ProjectService(_context);
        _controller = new ProjectsController(_projectService);
    }

    [Fact]
    public async Task GetUserProjects_ShouldReturnProjects_WhenUserHasProjects()
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
        var result = await _controller.GetUserProjects(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<ProjectResponseDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var projects = Assert.IsAssignableFrom<IEnumerable<ProjectResponseDto>>(actionResult.Value);
        Assert.Equal(2, projects.Count());
    }

    [Fact]
    public async Task GetUserProjects_ShouldReturnEmptyList_WhenUserHasNoProjects()
    {
        // Act
        var result = await _controller.GetUserProjects(999);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<ProjectResponseDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var projects = Assert.IsAssignableFrom<IEnumerable<ProjectResponseDto>>(actionResult.Value);
        Assert.Empty(projects);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnCreated_WhenValidRequest()
    {
        // Arrange
        var createProjectDto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "New Description",
            UserId = 1
        };

        // Act
        var result = await _controller.CreateProject(createProjectDto);

        // Assert
        var createdResult = Assert.IsType<ActionResult<ProjectResponseDto>>(result);
        var actionResult = Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        var project = Assert.IsType<ProjectResponseDto>(actionResult.Value);
        Assert.Equal("New Project", project.Name);
        Assert.Equal("New Description", project.Description);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnBadRequest_WhenInvalidModel()
    {
        // Arrange
        var createProjectDto = new CreateProjectDto
        {
            Name = "", // Invalid - empty name
            Description = "Description",
            UserId = 1
        };

        // Simulate model state error
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.CreateProject(createProjectDto);

        // Assert
        var badRequestResult = Assert.IsType<ActionResult<ProjectResponseDto>>(result);
        var actionResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        Assert.NotNull(actionResult.Value);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnUpdatedProject_WhenValidRequest()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Name = "Original Project",
            Description = "Original Description",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var updateProjectDto = new UpdateProjectDto
        {
            Name = "Updated Project",
            Description = "Updated Description"
        };

        // Act
        var result = await _controller.UpdateProject(1, 1, updateProjectDto);

        // Assert
        var okResult = Assert.IsType<ActionResult<ProjectResponseDto>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var updatedProject = Assert.IsType<ProjectResponseDto>(actionResult.Value);
        Assert.Equal("Updated Project", updatedProject.Name);
        Assert.Equal("Updated Description", updatedProject.Description);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var updateProjectDto = new UpdateProjectDto
        {
            Name = "Updated Project",
            Description = "Updated Description"
        };

        // Act
        var result = await _controller.UpdateProject(999, 1, updateProjectDto);

        // Assert
        var notFoundResult = Assert.IsType<ActionResult<ProjectResponseDto>>(result);
        var actionResult = Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        Assert.Equal("Projeto não encontrado", actionResult.Value);
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnNoContent_WhenProjectCanBeDeleted()
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
        var result = await _controller.DeleteProject(1, 1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.False(_context.Projects.Any(p => p.Id == 1));
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnBadRequest_WhenProjectHasPendingTasks()
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
        var result = await _controller.DeleteProject(1, 1);

        // Assert
        var badRequestResult = Assert.IsType<ActionResult>(result);
        var actionResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        Assert.Contains("tarefas pendentes", actionResult.Value?.ToString());
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Act
        var result = await _controller.DeleteProject(999, 1);

        // Assert
        var notFoundResult = Assert.IsType<ActionResult>(result);
        var actionResult = Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        Assert.Equal("Projeto não encontrado", actionResult.Value);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
