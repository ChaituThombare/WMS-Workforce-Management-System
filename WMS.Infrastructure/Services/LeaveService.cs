using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Leave;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public LeaveService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<LeaveResponseDto> ApplyAsync(LeaveApplyDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                LeaveType = dto.LeaveType,
                Reason = dto.Reason,
                Status = "Pending"
            };

            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();

            return new LeaveResponseDto
            {
                LeaveRequestId = leave.LeaveRequestId,
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.FirstName + " " + employee.LastName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                LeaveType = leave.LeaveType,
                Status = leave.Status,
                AppliedOn = leave.AppliedOn
            };
        }

        public async Task<List<LeaveResponseDto>> GetAllAsync()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Select(l => new LeaveResponseDto
                {
                    LeaveRequestId = l.LeaveRequestId,
                    EmployeeId = l.EmployeeId,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    LeaveType = l.LeaveType,
                    Status = l.Status,
                    ManagerComments = l.ManagerComments,
                    ApprovedBy = l.ApprovedBy,
                    AppliedOn = l.AppliedOn,
                    ApprovedOn = l.ApprovedOn
                })
                .ToListAsync();
        }

        public async Task<List<LeaveResponseDto>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId)
                .Select(l => new LeaveResponseDto
                {
                    LeaveRequestId = l.LeaveRequestId,
                    EmployeeId = l.EmployeeId,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    LeaveType = l.LeaveType,
                    Status = l.Status,
                    ManagerComments = l.ManagerComments,
                    AppliedOn = l.AppliedOn,
                    ApprovedOn = l.ApprovedOn,
                    ApprovedBy = l.ApprovedBy
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveAsync(int id, LeaveDecisionDto dto)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return false;

            leave.Status = "Approved";
            leave.ManagerComments = dto.ManagerComments;
            leave.ApprovedBy = dto.ApprovedBy;
            leave.ApprovedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "LeaveRequests",
                leave.LeaveRequestId,
                "Update",
                1
            );
            return true;
        }

        public async Task<bool> RejectAsync(int id, LeaveDecisionDto dto)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return false;

            leave.Status = "Rejected";
            leave.ManagerComments = dto.ManagerComments;
            leave.ApprovedBy = dto.ApprovedBy;
            leave.ApprovedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "LeaveRequests",
                leave.LeaveRequestId,
                "Update",
                1
            );
            return true;
        }

        public async Task<bool> CancelAsync(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return false;

            leave.Status = "Cancelled";

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "LeaveRequests",
                leave.LeaveRequestId,
                "Update",
                1
            );
            return true;
        }
    }
}
