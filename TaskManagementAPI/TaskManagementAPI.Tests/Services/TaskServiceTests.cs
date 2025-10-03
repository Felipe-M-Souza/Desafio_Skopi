using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Services;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Tests.Services;

public class TaskServiceTests : IDisposable
{
    private readonly TaskManagementDbContext _context;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementDbContext(options);
        _taskService = new TaskService(_context);
    }

    [Fact]
    public async Task CanCreateTaskAsync_ShouldReturnTrue_WhenProjectHasLessThan20Tasks()
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
        var result = await _taskService.CanCreateTaskAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanCreateTaskAsync_ShouldReturnFalse_WhenProjectHas20Tasks()
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

        // Act
        var result = await _taskService.CanCreateTaskAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask_WhenValidData()
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
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            DueDate = DateTime.Parse("2024-12-31"),
            ProjectId = 1,
            UserId = 1
        };

        // Act
        var result = await _taskService.CreateTaskAsync(createTaskDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal("High", result.Priority);
        Assert.Equal(1, result.ProjectId);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowException_WhenProjectHas20Tasks()
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
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            DueDate = DateTime.Parse("2024-12-31"),
            ProjectId = 1,
            UserId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _taskService.CreateTaskAsync(createTaskDto));
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenValidData()
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
            Priority = "High"
        };

        // Act
        var result = await _taskService.UpdateTaskAsync(1, updateTaskDto, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Task", result.Title);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal("InProgress", result.Status);
        Assert.Equal("High", result.Priority);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrowException_WhenChangingPriority()
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
            Priority = "High" // Different from original "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _taskService.UpdateTaskAsync(1, updateTaskDto, 1));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
