using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs.Project;

namespace WMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllAsync();

        Task<ProjectResponseDto?> GetByIdAsync(int id);

        Task<ProjectResponseDto> CreateAsync(ProjectCreateDto dto);

        Task<bool> UpdateAsync(int id, ProjectCreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
