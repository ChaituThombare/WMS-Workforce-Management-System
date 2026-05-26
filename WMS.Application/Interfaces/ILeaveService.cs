using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Leave;

namespace WMS.Application.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveResponseDto> ApplyAsync(LeaveApplyDto dto);

        Task<List<LeaveResponseDto>> GetAllAsync();

        Task<List<LeaveResponseDto>> GetByEmployeeAsync(int employeeId);

        Task<bool> ApproveAsync(int id, LeaveDecisionDto dto);

        Task<bool> RejectAsync(int id, LeaveDecisionDto dto);

        Task<bool> CancelAsync(int id);
    }
}
