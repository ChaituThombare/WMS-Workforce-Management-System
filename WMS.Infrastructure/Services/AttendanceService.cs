using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Attendance;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceResponseDto> CheckInAsync(AttendanceCheckInDto dto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId);

            if (employee == null)
                throw new Exception("Employee not found.");

            var today = DateTime.Now.Date;

            var existingAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == dto.EmployeeId && a.AttendanceDate == today);

            if (existingAttendance != null)
                throw new Exception("Employee already checked in today");

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                AttendanceDate = today,
                CheckInTime = DateTime.Now,
                TotalHours = 0,
                WorkMode = dto.WorkMode
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return new AttendanceResponseDto
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = employee.EmployeeId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                AttendanceDate = attendance.AttendanceDate,
                CheckInTime = attendance.CheckInTime,
                CheckOutTime = attendance.CheckOutTime,
                TotalHours = attendance.TotalHours,
                WorkMode = attendance.WorkMode
            };
        }

        public async Task<AttendanceResponseDto?> CheckOutAsync(AttendanceCheckOutDto dto)
        {
            var today = DateTime.Now.Date;

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == dto.EmployeeId && a.AttendanceDate == today);

            if (attendance == null)
                return null;

            if (attendance.CheckOutTime != null)
                throw new Exception("Employee already checked out today.");

            attendance.CheckOutTime = DateTime.Now;

            attendance.TotalHours = (attendance.CheckOutTime.Value - attendance.CheckInTime).TotalHours;

            await _context.SaveChangesAsync();

            return new AttendanceResponseDto
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = attendance.EmployeeId,
                EmployeeName = $"{attendance.Employee.FirstName} {attendance.Employee.LastName}",
                AttendanceDate = attendance.AttendanceDate,
                CheckInTime = attendance.CheckInTime,
                CheckOutTime = attendance.CheckOutTime,
                TotalHours = Math.Round(attendance.TotalHours, 2),
                WorkMode = attendance.WorkMode
            };
        }

        public async Task<List<AttendanceResponseDto>> GetAllAsync()
        {
            return await _context.Attendances
                    .Include(a => a.Employee)
                    .Select(a => new AttendanceResponseDto
                    {
                        AttendanceId = a.AttendanceId,
                        EmployeeId = a.EmployeeId,
                        EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                        AttendanceDate = a.AttendanceDate,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime,
                        TotalHours = Math.Round(a.TotalHours, 2),
                        WorkMode = a.WorkMode
                    })
                    .ToListAsync();
        }
        public async Task<List<AttendanceResponseDto>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Attendances
                    .Include(a => a.Employee)
                    .Where(a => a.EmployeeId == employeeId)
                    .Select(a => new AttendanceResponseDto
                    {
                        AttendanceId = a.AttendanceId,
                        EmployeeId = a.EmployeeId,
                        EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                        AttendanceDate = a.AttendanceDate,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime,
                        TotalHours = Math.Round(a.TotalHours, 2),
                        WorkMode = a.WorkMode
                    })
                    .ToListAsync();
        }
    }
}
