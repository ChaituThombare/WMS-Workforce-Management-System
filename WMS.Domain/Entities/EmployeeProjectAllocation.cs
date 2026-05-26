using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class EmployeeProjectAllocation
    {
        public int AllocationId { get; set; }

        public int EmpId { get; set; }

        public int ProjectId { get; set; }

        public DateTime AssignedOn { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public bool Status { get; set; } = true;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Employee Employee { get; set; } = null!;

        public Project Project { get; set; } = null!;
    }
}
