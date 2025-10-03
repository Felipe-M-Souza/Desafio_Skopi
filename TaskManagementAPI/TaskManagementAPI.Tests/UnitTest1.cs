using Xunit;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var expected = true;
        
        // Act
        var actual = true;
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void User_ShouldHaveRequiredProperties()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("Test User", user.Name);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("User", user.Role);
    }

    [Fact]
    public void Project_ShouldHaveRequiredProperties()
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

        // Act & Assert
        Assert.Equal(1, project.Id);
        Assert.Equal("Test Project", project.Name);
        Assert.Equal("Test Description", project.Description);
        Assert.Equal(1, project.UserId);
    }

    [Fact]
    public void Task_ShouldHaveRequiredProperties()
    {
        // Arrange
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

        // Act & Assert
        Assert.Equal(1, task.Id);
        Assert.Equal("Test Task", task.Title);
        Assert.Equal("Test Description", task.Description);
        Assert.Equal("Pending", task.Status);
        Assert.Equal("Medium", task.Priority);
        Assert.Equal(1, task.ProjectId);
        Assert.Equal(1, task.UserId);
    }

    [Fact]
    public void TaskHistory_ShouldHaveRequiredProperties()
    {
        // Arrange
        var taskHistory = new TaskHistory
        {
            Id = 1,
            Comment = "Test Comment",
            Status = "Created",
            TaskId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Equal(1, taskHistory.Id);
        Assert.Equal("Test Comment", taskHistory.Comment);
        Assert.Equal("Created", taskHistory.Status);
        Assert.Equal(1, taskHistory.TaskId);
        Assert.Equal(1, taskHistory.UserId);
    }

    [Fact]
    public void CreateTaskDto_ShouldHaveRequiredProperties()
    {
        // Arrange
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
        Assert.Equal("Test Task", createTaskDto.Title);
        Assert.Equal("Test Description", createTaskDto.Description);
        Assert.Equal("High", createTaskDto.Priority);
        Assert.Equal(DateTime.Parse("2024-12-31"), createTaskDto.DueDate);
        Assert.Equal(1, createTaskDto.ProjectId);
        Assert.Equal(1, createTaskDto.UserId);
    }

    [Fact]
    public void UpdateTaskDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var updateTaskDto = new UpdateTaskDto
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Status = "InProgress",
            DueDate = DateTime.Parse("2024-12-31"),
            Priority = "Medium"
        };

        // Act & Assert
        Assert.Equal("Updated Task", updateTaskDto.Title);
        Assert.Equal("Updated Description", updateTaskDto.Description);
        Assert.Equal("InProgress", updateTaskDto.Status);
        Assert.Equal(DateTime.Parse("2024-12-31"), updateTaskDto.DueDate);
        Assert.Equal("Medium", updateTaskDto.Priority);
    }

    [Fact]
    public void CreateProjectDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var createProjectDto = new CreateProjectDto
        {
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1
        };

        // Act & Assert
        Assert.Equal("Test Project", createProjectDto.Name);
        Assert.Equal("Test Description", createProjectDto.Description);
        Assert.Equal(1, createProjectDto.UserId);
    }

    [Fact]
    public void TaskStatus_Enum_ShouldHaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal(0, (int)Models.TaskStatus.Pending);
        Assert.Equal(1, (int)Models.TaskStatus.InProgress);
        Assert.Equal(2, (int)Models.TaskStatus.Completed);
        Assert.Equal(3, (int)Models.TaskStatus.Cancelled);
    }

    [Fact]
    public void TaskPriority_Enum_ShouldHaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal(0, (int)TaskPriority.Low);
        Assert.Equal(1, (int)TaskPriority.Medium);
        Assert.Equal(2, (int)TaskPriority.High);
    }
}
