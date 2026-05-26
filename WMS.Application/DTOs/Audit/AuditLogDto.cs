using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Audit
{
    public class AuditLogDto
    {
        public int AuditId { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public int RecordId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}