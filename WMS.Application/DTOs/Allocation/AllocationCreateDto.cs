using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Allocation
{
    public class AllocationCreateDto
    {
        public int EmpId { get; set; }

        public int ProjectId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
