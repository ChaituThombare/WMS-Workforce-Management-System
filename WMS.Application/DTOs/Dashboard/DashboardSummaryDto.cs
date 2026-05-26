using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalClients { get; set; }

        public int TotalProjects { get; set; }

        public int ActiveAllocations { get; set; }

        public int PendingLeaves { get; set; }

        public int TodayAttendance { get; set; }
    }
}
