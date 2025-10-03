using Xunit;
using TaskManagementAPI.Models;

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
}
