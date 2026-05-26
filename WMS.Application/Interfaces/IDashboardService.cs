using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Dashboard;
namespace WMS.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
