using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Project;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public ProjectService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Client)
                .Select(p => new ProjectResponseDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ClientId = p.ClientId,
                    ClientName = p.Client != null ? p.Client.ClientName : null,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status
                }).ToListAsync();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                return null;

            return new ProjectResponseDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ClientId = project.ClientId,
                ClientName = project.Client?.ClientName,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status
            };
        }

        public async Task<ProjectResponseDto> CreateAsync(ProjectCreateDto dto)
        {
            var project = new Project
            {
                ProjectName = dto.ProjectName,
                ClientId = dto.ClientId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Projects",
                project.ProjectId,
                "Insert",
                1
            );
            var created = await GetByIdAsync(project.ProjectId);
            return created!;
        }

        public async Task<bool> UpdateAsync(int id, ProjectCreateDto dto)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return false;

            project.ProjectName = dto.ProjectName;
            project.ClientId = dto.ClientId;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.Status = dto.Status;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Projects",
                project.ProjectId,
                "Update",
                1
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return false;

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Projects",
                project.ProjectId,
                "Delete",
                1
            );
            return true;
        }
    }
}
