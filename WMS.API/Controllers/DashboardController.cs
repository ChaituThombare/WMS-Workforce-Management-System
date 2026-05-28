using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        [Authorize]
        public async Task<IActionResult> GetSummary()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var employeeIdClaim = User.FindFirst
                ("EmployeeId")?.Value;

            int? employeeId = null;

            if (!string.IsNullOrEmpty(employeeIdClaim))
            {
                employeeId = int.Parse(employeeIdClaim);
            }

            var result = await _dashboardService
                .GetSummaryAsync(role!, employeeId);

            return Ok(result);
        }
    }
}
