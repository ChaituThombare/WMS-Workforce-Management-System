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

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var today = DateTime.Now.Date;

            return new DashboardSummaryDto
            {
                TotalEmployees = await _context.Employees.CountAsync(),

                TotalDepartments = await _context.Departments.CountAsync(),

                TotalClients = await _context.Clients.CountAsync(),

                TotalProjects = await _context.Projects.CountAsync(),

                ActiveAllocations = await _context.EmployeeProjectAllocations
                    .CountAsync(a => a.Status == true),

                PendingLeaves = await _context.LeaveRequests
                    .CountAsync(l => l.Status == "Pending"),

                TodayAttendance = await _context.Attendances
                    .CountAsync(a => a.AttendanceDate == today)
            };
        }
    }
}
