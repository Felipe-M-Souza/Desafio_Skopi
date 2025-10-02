namespace TaskManagementAPI.DTOs
{
    public class UserTaskReportDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public double AverageCompletedTasks { get; set; }
        public int TotalCompletedTasks { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
