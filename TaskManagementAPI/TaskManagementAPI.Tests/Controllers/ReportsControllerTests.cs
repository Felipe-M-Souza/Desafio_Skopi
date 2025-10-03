using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Controllers;

public class ReportsControllerTests : IDisposable
{
    private readonly TaskManagementDbContext _context;
    private readonly ReportService _reportService;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementDbContext(options);
        _reportService = new ReportService(_context);
        _controller = new ReportsController(_reportService);
    }

    [Fact]
    public async Task GetUserTaskReport_ShouldReturnReport_WhenUserHasTasks()
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
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow.AddDays(-10) // Within 30 days
        };

        _context.Users.Add(user);
        _context.Projects.Add(project);
        _context.Tasks.Add(completedTask);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetUserTaskReport(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<UserTaskReportDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var reports = Assert.IsAssignableFrom<IEnumerable<UserTaskReportDto>>(actionResult.Value);
        Assert.Single(reports);
        
        var report = reports.First();
        Assert.Equal(1, report.UserId);
        Assert.Equal("Test User", report.UserName);
        Assert.Equal("test@example.com", report.UserEmail);
        Assert.Equal(1, report.TotalCompletedTasks);
    }

    [Fact]
    public async Task GetUserTaskReport_ShouldReturnEmptyReport_WhenUserHasNoTasks()
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

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetUserTaskReport(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<UserTaskReportDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var reports = Assert.IsAssignableFrom<IEnumerable<UserTaskReportDto>>(actionResult.Value);
        Assert.Single(reports);
        
        var report = reports.First();
        Assert.Equal(0, report.TotalCompletedTasks);
    }

    [Fact]
    public async Task GetUserTaskReport_ShouldReturnAllUsers_WhenMultipleUsersExist()
    {
        // Arrange
        var user1 = new User
        {
            Id = 1,
            Name = "User 1",
            Email = "user1@example.com",
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        var user2 = new User
        {
            Id = 2,
            Name = "User 2",
            Email = "user2@example.com",
            Role = "Manager",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetUserTaskReport(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<UserTaskReportDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var reports = Assert.IsAssignableFrom<IEnumerable<UserTaskReportDto>>(actionResult.Value);
        Assert.Equal(2, reports.Count());
        
        Assert.Contains(reports, r => r.UserName == "User 1");
        Assert.Contains(reports, r => r.UserName == "User 2");
    }

    [Fact]
    public async Task GetUserTaskReport_ShouldFilterByDate_WhenTasksAreOutside30Days()
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

        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "Test Description",
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        var recentTask = new Models.Task
        {
            Id = 1,
            Title = "Recent Task",
            Description = "Description",
            Status = "Completed",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow.AddDays(-10) // Within 30 days
        };

        var oldTask = new Models.Task
        {
            Id = 2,
            Title = "Old Task",
            Description = "Description",
            Status = "Completed",
            Priority = "Medium",
            ProjectId = 1,
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow.AddDays(-40) // Outside 30 days
        };

        _context.Users.Add(user);
        _context.Projects.Add(project);
        _context.Tasks.AddRange(recentTask, oldTask);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetUserTaskReport(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<UserTaskReportDto>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var reports = Assert.IsAssignableFrom<IEnumerable<UserTaskReportDto>>(actionResult.Value);
        Assert.Single(reports);
        
        var report = reports.First();
        Assert.Equal(1, report.TotalCompletedTasks); // Only the recent one
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
