using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.DTOs.Client;

namespace WMS.Application.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientResponseDto>> GetAllAsync();

        Task<ClientResponseDto?> GetByIdAsync(int id);

        Task<ClientResponseDto> CreateAsync(ClientCreateDto dto);

        Task<bool> UpdateAsync(int id, ClientCreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
