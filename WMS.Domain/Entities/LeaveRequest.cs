using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string LeaveType { get; set; } = "Casual";

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public string? ManagerComments { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public DateTime AppliedOn { get; set; } = DateTime.Now;

        public Employee Employee { get; set; } = null!;
    }
}
