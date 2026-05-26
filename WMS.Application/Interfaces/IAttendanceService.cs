using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Attendance;

namespace WMS.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto> CheckInAsync(AttendanceCheckInDto dto);

        Task<AttendanceResponseDto?> CheckOutAsync(AttendanceCheckOutDto dto);

        Task<List<AttendanceResponseDto>> GetAllAsync();

        Task<List<AttendanceResponseDto>> GetByEmployeeAsync(int employeeId);
    }
}
