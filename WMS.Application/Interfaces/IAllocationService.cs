using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs.Allocation;

namespace WMS.Application.Interfaces
{
    public interface IAllocationService
    {
        Task<List<AllocationResponseDto>> GetAllAsync();

        Task<AllocationResponseDto> CreateAsync(AllocationCreateDto dto);

        Task<bool> DeallocateAsync(int id);

        Task<List<AllocationResponseDto>> GetByEmployeeAsync(int employeeId);
    }
}
