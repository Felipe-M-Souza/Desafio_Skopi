using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    public interface IReportService
    {
        Task<IEnumerable<UserTaskReportDto>> GetUserTaskReportAsync(int managerUserId);
        Task<bool> IsManagerAsync(int userId);
    }
}
