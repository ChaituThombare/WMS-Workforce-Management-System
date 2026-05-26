using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Allocation;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AllocationsController : ControllerBase
    {
        private readonly IAllocationService _allocationService;

        public AllocationsController(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _allocationService.GetAllAsync());
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetByEmployee(int id)
        {
            var allocations = await _allocationService.GetByEmployeeAsync(id);
            return Ok(allocations);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AllocationCreateDto dto)
        {
            return Ok(await _allocationService.CreateAsync(dto));
        }

        [HttpPut("deallocate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deallocate(int id)
        {
            var result = await _allocationService.DeallocateAsync(id);

            if (!result)
                return NotFound();

            return Ok(new { message = "Allocation deactivated" });
        }
    }
}
