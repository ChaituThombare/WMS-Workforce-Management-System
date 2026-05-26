using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Department;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public DepartmentService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<DepartmentResponseDto>> GetAllAsync()
        {
            return await _context.Departments
                .Select(d => new DepartmentResponseDto
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Description = d.Description,
                    CreatedOn = d.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (department == null)
            {
                return null;
            }
            return new DepartmentResponseDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                CreatedOn = department.CreatedOn
            };
        }

        public async Task<DepartmentResponseDto> CreateAsync(DepartmentCreateDto dto)
        {
            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description,
                CreatedOn = DateTime.Now
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Departments",
                department.DepartmentId,
                "Insert",
                1
            );

            return new DepartmentResponseDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                CreatedOn = department.CreatedOn
            };
        }

        public async Task<bool> UpdateAsync(int id, DepartmentUpdateDto dto)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return false;
            }

            department.DepartmentName = dto.DepartmentName;
            department.Description = dto.Description;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Departments",
                department.DepartmentId,
                "Update",
                1
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Departments",
                department.DepartmentId,
                "Delete",
                1
            );
  
            return true;
        }
    }
}
