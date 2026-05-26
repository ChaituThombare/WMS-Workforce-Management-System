using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WMS.Application.DTOs.Audit;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(
            string entityName,
            int recordId,
            string action,
            int createdBy)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier);

            int actualUserId = 0;

            if (userIdClaim != null)
            {
                int.TryParse(userIdClaim.Value, out actualUserId);
            }

            var log = new AuditLog
            {
                EntityName = entityName,
                RecordId = recordId,
                Action = action,
                CreatedBy = actualUserId,
                CreatedOn = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLogDto>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new AuditLogDto
                {
                    AuditId = x.AuditId,
                    EntityName = x.EntityName,
                    RecordId = x.RecordId,
                    Action = x.Action,
                    CreatedBy = _context.UserLogins
                                .Where(u => u.UserId == x.CreatedBy)
                                .Select(u => u.Username)
                                .FirstOrDefault() ?? "System",
                    CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }
    }
}