using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Announcement;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _announcementService.GetAllAsync());
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _announcementService.GetActiveAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementDto dto)
        {
            return Ok(await _announcementService.CreateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("toggle/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var updated = await _announcementService.ToggleStatusAsync(id);

            if (!updated)
                return NotFound();

            return Ok();
        }
    }
}