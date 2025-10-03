using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Controllers;

public class TasksControllerTests : IDisposable
{
    private readonly TaskManagementDbContext _context;
    private readonly TaskService _taskService;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementDbContext(options);
        _taskService = new TaskService(_context);
        _controller = new TasksController(_taskService);
    }

    [Fact]
    public async Task GetProjectTasks_ShouldReturnTasks_WhenProjectExists()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetProjectTasks(1, 1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TaskResponseDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskResponseDto>>(actionResult.Value);
        Assert.Single(tasks);
    }

    [Fact]
    public async Task GetProjectTasks_ShouldReturnEmptyList_WhenProjectHasNoTasks()
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
        var result = await _controller.GetProjectTasks(1, 1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TaskResponseDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskResponseDto>>(actionResult.Value);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task GetTask_ShouldReturnTask_WhenTaskExists()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetTask(1, 1);

        // Assert
        var okResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var taskResult = Assert.IsType<TaskResponseDto>(actionResult.Value);
        Assert.Equal("Test Task", taskResult.Title);
    }

    [Fact]
    public async Task GetTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        // Act
        var result = await _controller.GetTask(999, 1);

        // Assert
        var notFoundResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        Assert.Equal("Tarefa não encontrada", actionResult.Value);
    }

    [Fact]
    public async Task CreateTask_ShouldReturnCreated_WhenValidRequest()
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

        var createTaskDto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "New Description",
            Priority = "High",
            DueDate = DateTime.Parse("2024-12-31"),
            ProjectId = 1,
            UserId = 1
        };

        // Act
        var result = await _controller.CreateTask(createTaskDto);

        // Assert
        var createdResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        var task = Assert.IsType<TaskResponseDto>(actionResult.Value);
        Assert.Equal("New Task", task.Title);
    }

    [Fact]
    public async Task CreateTask_ShouldReturnBadRequest_WhenProjectHas20Tasks()
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

        // Add 20 tasks
        for (int i = 1; i <= 20; i++)
        {
            var task = new Models.Task
            {
                Id = i,
                Title = $"Task {i}",
                Description = $"Description {i}",
                Status = "Pending",
                Priority = "Medium",
                ProjectId = 1,
                UserId = 1,
                CreatedAt = DateTime.UtcNow
            };
            _context.Tasks.Add(task);
        }
        await _context.SaveChangesAsync();

        var createTaskDto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "New Description",
            Priority = "High",
            DueDate = DateTime.Parse("2024-12-31"),
            ProjectId = 1,
            UserId = 1
        };

        // Act
        var result = await _controller.CreateTask(createTaskDto);

        // Assert
        var badRequestResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        Assert.Contains("limite máximo de 20 tarefas", actionResult.Value?.ToString());
    }

    [Fact]
    public async Task UpdateTask_ShouldReturnUpdatedTask_WhenValidRequest()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Original Task",
            Description = "Original Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var updateTaskDto = new UpdateTaskDto
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Status = "InProgress",
            DueDate = DateTime.Parse("2024-12-31"),
            Priority = "Medium" // Same priority
        };

        // Act
        var result = await _controller.UpdateTask(1, 1, updateTaskDto);

        // Assert
        var okResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var updatedTask = Assert.IsType<TaskResponseDto>(actionResult.Value);
        Assert.Equal("Updated Task", updatedTask.Title);
        Assert.Equal("InProgress", updatedTask.Status);
    }

    [Fact]
    public async Task UpdateTask_ShouldReturnBadRequest_WhenChangingPriority()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Original Task",
            Description = "Original Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var updateTaskDto = new UpdateTaskDto
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Status = "InProgress",
            DueDate = DateTime.Parse("2024-12-31"),
            Priority = "High" // Different priority
        };

        // Act
        var result = await _controller.UpdateTask(1, 1, updateTaskDto);

        // Assert
        var badRequestResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        Assert.Contains("não é permitido alterar a prioridade", actionResult.Value?.ToString());
    }

    [Fact]
    public async Task DeleteTask_ShouldReturnNoContent_WhenTaskExists()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteTask(1, 1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.False(_context.Tasks.Any(t => t.Id == 1));
    }

    [Fact]
    public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        // Act
        var result = await _controller.DeleteTask(999, 1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Tarefa não encontrada", notFoundResult.Value);
    }

    [Fact]
    public async Task AddTaskComment_ShouldReturnTask_WhenValidRequest()
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

        var task = new Models.Task
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Status = "Pending",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var addCommentDto = new AddTaskCommentDto
        {
            Comment = "Test Comment",
            UserId = 1
        };

        // Act
        var result = await _controller.AddTaskComment(1, addCommentDto);

        // Assert
        var okResult = Assert.IsType<ActionResult<TaskResponseDto>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var taskResult = Assert.IsType<TaskResponseDto>(actionResult.Value);
        Assert.Equal("Test Task", taskResult.Title);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
