using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Dashboard;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(string role, int? employeeId)
        {
            var today = DateTime.Now.Date;

            var response = new DashboardSummaryDto
            {
                // ADMIN METRICS
                TotalEmployees = await _context.Employees.CountAsync(),

                ActiveEmployees = await _context.Employees
                    .CountAsync(e => e.Status == "Active"),

                TotalProjects = await _context.Projects.CountAsync(),

                ActiveProjects = await _context.Projects
                    .CountAsync(p => p.Status == "Active"),

                ActiveAllocations = await _context.EmployeeProjectAllocations
                    .CountAsync(a => a.Status == true),

                PendingLeaves = await _context.LeaveRequests
                    .CountAsync(l => l.Status == "Pending"),

                TodayAttendance = await _context.Attendances
                    .CountAsync(a => a.AttendanceDate == today),

                EmployeesOnLeave = await _context.LeaveRequests.CountAsync(l => l.Status == "Approved" && l.StartDate.Date <= today && l.EndDate.Date >= today)
            };

            // EMPLOYEE SPECIFIC METRICS
            if (role == "Employee" && employeeId.HasValue)
            {
                response.MyProjects = await _context.EmployeeProjectAllocations
                    .CountAsync(a =>
                        a.EmpId == employeeId.Value &&
                        a.Status == true);

                response.MyPendingLeaves = await _context.LeaveRequests
                    .CountAsync(l =>
                        l.EmployeeId == employeeId.Value &&
                        l.Status == "Pending");

                response.MyAttendanceCount = await _context.Attendances
                    .CountAsync(a =>
                        a.EmployeeId == employeeId.Value);

                var avgHours = await _context.Attendances
                    .Where(a => a.EmployeeId == employeeId.Value)
                    .AverageAsync(a => (double?)a.TotalHours);

                response.MyAverageHours = Math.Round(avgHours ?? 0, 2);
            }

            return response;
        }
    }
}
