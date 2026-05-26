using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class AuditLog
    {
        public int AuditId { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public int RecordId { get; set; }

        public string Action { get; set; } = string.Empty;

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}