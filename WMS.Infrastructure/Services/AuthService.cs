using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Application.DTOs.Auth;
using WMS.Infrastructure.Persistence;
using WMS.Infrastructure.Identity;

using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(ApplicationDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.UserLogins
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return null;

            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            if (user.PasswordHash != hashedPassword)
                return null;

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user, user.Role.RoleName);

            string fullName = "Admin";

            if (user.EmployeeId.HasValue)
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == user.EmployeeId.Value);

                if (employee != null)
                {
                    fullName = employee.FirstName + " " + employee.LastName;
                }
            }

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role.RoleName,
                FullName = fullName,
                EmployeeId = user.EmployeeId
            };
        }
    }
}
