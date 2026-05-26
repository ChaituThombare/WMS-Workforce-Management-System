using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Employee;

namespace WMS.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllAsync();

        Task<EmployeeResponseDto?> GetByIdAsync(int id);

        Task<EmployeeResponseDto> CreateAsync(EmployeeCreateDto dto);

        Task<bool> UpdateAsync(int id, EmployeeUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
