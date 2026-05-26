using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Announcement;

namespace WMS.Application.Interfaces
{
    public interface IAnnouncementService
    {
        Task<List<AnnouncementDto>> GetAllAsync();

        Task<List<AnnouncementDto>> GetActiveAsync();

        Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto);

        Task<bool> ToggleStatusAsync(int id);
    }
}