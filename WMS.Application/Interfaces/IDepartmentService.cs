using System;
using System.Collections.Generic;

using WMS.Application.DTOs.Department;

namespace WMS.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponseDto>> GetAllAsync();

        Task<DepartmentResponseDto?> GetByIdAsync(int id);

        Task<DepartmentResponseDto> CreateAsync(DepartmentCreateDto dto);
        
        Task<bool> UpdateAsync(int id, DepartmentUpdateDto dto);
        
        Task<bool> DeleteAsync(int id);
    }
}
