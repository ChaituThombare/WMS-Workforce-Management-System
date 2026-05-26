using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Leave
{
    public class LeaveDecisionDto
    {
        public string ManagerComments { get; set; } = string.Empty;

        public string ApprovedBy { get; set; } = "admin";
    }
}
