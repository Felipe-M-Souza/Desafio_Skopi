using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        
        /// <summary>
        /// Obtém relatório de tarefas concluídas por usuário (apenas para gerentes)
        /// </summary>
        [HttpGet("user-tasks/{managerUserId}")]
        public async Task<ActionResult<IEnumerable<UserTaskReportDto>>> GetUserTaskReport(int managerUserId)
        {
            try
            {
                var report = await _reportService.GetUserTaskReportAsync(managerUserId);
                return Ok(report);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao gerar relatório: {ex.Message}");
            }
        }
    }
}
