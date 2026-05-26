using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Allocation
{
    public class AllocationResponseDto
    {
        public int AllocationId { get; set; }

        public int EmpId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public DateTime AssignedOn { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public bool Status { get; set; }
    }
}
