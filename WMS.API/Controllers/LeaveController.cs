using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Leave;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _leaveService.GetAllAsync());
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            return Ok(await _leaveService.GetByEmployeeAsync(employeeId));
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply(LeaveApplyDto dto)
        {
            return Ok(await _leaveService.ApplyAsync(dto));
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id, LeaveDecisionDto dto)
        {
            var result = await _leaveService.ApproveAsync(id, dto);

            if (!result)
                return NotFound();

            return Ok(new { message = "Leave approved" });
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id, LeaveDecisionDto dto)
        {
            var result = await _leaveService.RejectAsync(id, dto);

            if (!result)
                return NotFound();

            return Ok(new { message = "Leave rejected" });
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _leaveService.CancelAsync(id);

            if (!result)
                return NotFound();

            return Ok(new { message = "Leave cancelled" });
        }
    }
}
