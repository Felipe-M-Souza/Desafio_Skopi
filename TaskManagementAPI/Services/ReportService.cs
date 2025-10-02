using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly TaskManagementDbContext _context;
        
        public ReportService(TaskManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<UserTaskReportDto>> GetUserTaskReportAsync(int managerUserId)
        {
            // Verify if user is a manager
            if (!await IsManagerAsync(managerUserId))
                throw new UnauthorizedAccessException("Apenas gerentes podem acessar este relatório");
            
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            
            var report = await _context.Users
                .Where(u => u.Role == "User") // Only regular users, not managers
                .Select(u => new UserTaskReportDto
                {
                    UserId = u.Id,
                    UserName = u.Name,
                    UserEmail = u.Email,
                    TotalCompletedTasks = u.Tasks.Count(t => t.Status == Models.TaskStatus.Completed && t.UpdatedAt >= thirtyDaysAgo),
                    ReportDate = DateTime.UtcNow
                })
                .ToListAsync();
            
            // Calculate average for each user
            foreach (var userReport in report)
            {
                userReport.AverageCompletedTasks = userReport.TotalCompletedTasks / 30.0;
            }
            
            return report;
        }
        
        public async Task<bool> IsManagerAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Role == "Manager";
        }
    }
}
