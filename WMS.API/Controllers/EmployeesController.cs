using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WMS.Application.Interfaces;
using WMS.Application.DTOs.Employee;
using System.Security.Claims;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeClaim = User.FindFirst("EmployeeId")?.Value;

            if (role == "Employee")
            {
                if (string.IsNullOrEmpty(employeeClaim) || int.Parse(employeeClaim) != id)
                {
                    return Forbid();
                }
            }

            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(EmployeeCreateDto dto)
        {
            var created = await _employeeService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
        {
            var updated = await _employeeService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return Ok(new { message = "Employee updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}
