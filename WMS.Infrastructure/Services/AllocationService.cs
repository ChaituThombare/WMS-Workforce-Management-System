using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Allocation;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class AllocationService : IAllocationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public AllocationService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<AllocationResponseDto>> GetAllAsync()
        {
            return await _context.EmployeeProjectAllocations
                .Include(a => a.Employee)
                .Include(a => a.Project)
                .Select(a => new AllocationResponseDto
                {
                    AllocationId = a.AllocationId,
                    EmpId = a.EmpId,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    ProjectId = a.ProjectId,
                    ProjectName = a.Project.ProjectName,
                    AssignedOn = a.AssignedOn,
                    CreatedBy = a.CreatedBy,
                    Status = a.Status
                }).ToListAsync();
        }

        public async Task<List<AllocationResponseDto>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeProjectAllocations
                .Include(a => a.Employee)
                .Include(a => a.Project)
                .Where(a => a.EmpId == employeeId)
                .Select(a => new AllocationResponseDto
                {
                    AllocationId = a.AllocationId,
                    EmpId = a.EmpId,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    ProjectId = a.ProjectId,
                    ProjectName = a.Project.ProjectName,
                    AssignedOn = a.AssignedOn,
                    CreatedBy = a.CreatedBy,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<AllocationResponseDto> CreateAsync(AllocationCreateDto dto)
        {
            var allocation = new EmployeeProjectAllocation
            {
                EmpId = dto.EmpId,
                ProjectId = dto.ProjectId,
                AssignedOn = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedBy = dto.CreatedBy,
                Status = true
            };

            _context.EmployeeProjectAllocations.Add(allocation);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "EmployeeProjectAllocations",
                allocation.AllocationId,
                "Insert",
                1
            );

            var employee = await _context.Employees.FindAsync(dto.EmpId);
            var project = await _context.Projects.FindAsync(dto.ProjectId);

            return new AllocationResponseDto
            {
                AllocationId = allocation.AllocationId,
                EmpId = allocation.EmpId,
                EmployeeName = employee!.FirstName + " " + employee.LastName,
                ProjectId = allocation.ProjectId,
                ProjectName = project!.ProjectName,
                AssignedOn = allocation.AssignedOn,
                CreatedBy = allocation.CreatedBy,
                Status = allocation.Status
            };
        }

        public async Task<bool> DeallocateAsync(int id)
        {
            var allocation = await _context.EmployeeProjectAllocations.FindAsync(id);

            if (allocation == null)
                return false;

            allocation.Status = false;
            allocation.UpdatedBy = "admin";
            allocation.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "EmployeeProjectAllocations",
                allocation.AllocationId,
                "Delete",
                1
            );
            return true;
        }
    }
}
