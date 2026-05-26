using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Employee;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;
using WMS.Infrastructure.Identity;

namespace WMS.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public EmployeeService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Gender = e.Gender,
                    DOB = e.DOB,
                    DOJ = e.DOJ,
                    DepartmentName = e.Department.DepartmentName,
                    RoleName = e.Role.RoleName,
                    Status = e.Status
                }).ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department).Include(e => e.Role).FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return null;
            }
            return new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DOJ = employee.DOJ,
                DepartmentName = employee.Department.DepartmentName,
                RoleName = employee.Role.RoleName,
                Status = employee.Status
            };
        }
        public async Task<EmployeeResponseDto> CreateAsync(EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,
                DOB = dto.DOB,
                DOJ = dto.DOJ,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                Status = "Active",
                CreatedOn = DateTime.Now
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var username = employee.FirstName.ToLower();

            var userLogin = new UserLogin
            {
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                EmployeeId = employee.EmployeeId
            };

            _context.UserLogins.Add(userLogin);
            
            await _context.SaveChangesAsync();
            await _auditService.LogAsync("Employee", employee.EmployeeId, "Insert", 1
            );

            var createdEmployee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstAsync(e => e.EmployeeId == employee.EmployeeId);

            return new EmployeeResponseDto
            {
                EmployeeId = createdEmployee.EmployeeId,
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                Gender = createdEmployee.Gender,
                DOB = createdEmployee.DOB,
                DOJ = createdEmployee.DOJ,
                DepartmentName = createdEmployee.Department.DepartmentName,
                RoleName = createdEmployee.Role.RoleName,
                Status = createdEmployee.Status
            };
        }

        public async Task<bool> UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return false;
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Gender = dto.Gender;
            employee.DepartmentId = dto.DepartmentId;
            employee.RoleId = dto.RoleId;
            employee.Status = string.IsNullOrWhiteSpace(dto.Status) ? employee.Status : dto.Status;
            employee.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Employees",
                employee.EmployeeId,
                "Update",
                1
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Employees",
                employee.EmployeeId,
                "Delete",
                1
            );
            return true;
        }
    }
}
