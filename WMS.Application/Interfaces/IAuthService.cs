using WMS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
