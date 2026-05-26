using System;
using System.Collections.Generic;
using WMS.Application.DTOs.Audit;

namespace WMS.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(
            string entityName,
            int recordId,
            string action,
            int createdBy
        );

        Task<List<AuditLogDto>> GetAllAsync();
    }
}