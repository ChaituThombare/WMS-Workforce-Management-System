using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        
        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }
    }
}
