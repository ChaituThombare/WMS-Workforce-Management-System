using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Attendance;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(AttendanceCheckInDto dto)
        {
            try
            {
                var result = await _attendanceService.CheckInAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut(AttendanceCheckOutDto dto)
        {
            try
            {
                var result = await _attendanceService.CheckOutAsync(dto);

                if (result == null)
                    return NotFound(new { message = "No check-in found for today" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var records = await _attendanceService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var records = await _attendanceService.GetByEmployeeAsync(employeeId);
            return Ok(records);
        }
    }
}
