using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Announcement;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public AnnouncementService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<AnnouncementDto>> GetAllAsync()
        {
            return await _context.Announcements
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new AnnouncementDto
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Message = a.Message,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        public async Task<List<AnnouncementDto>> GetActiveAsync()
        {
            return await _context.Announcements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new AnnouncementDto
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Message = a.Message,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto)
        {
            var announcement = new Announcement
            {
                Title = dto.Title,
                Message = dto.Message,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Announcements",
                announcement.AnnouncementId,
                "Insert",
                1
            );

            return new AnnouncementDto
            {
                AnnouncementId = announcement.AnnouncementId,
                Title = announcement.Title,
                Message = announcement.Message,
                CreatedBy = announcement.CreatedBy,
                CreatedOn = announcement.CreatedOn,
                IsActive = announcement.IsActive
            };
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null)
                return false;

            announcement.IsActive = !announcement.IsActive;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Announcements",
                announcement.AnnouncementId,
                "Update",
                1
            );
            return true;
        }
    }
}