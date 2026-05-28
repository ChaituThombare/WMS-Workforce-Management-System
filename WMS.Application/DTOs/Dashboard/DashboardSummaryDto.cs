using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        // Admin metrics
        public int TotalEmployees { get; set; }

        public int ActiveEmployees { get; set; }

        public int TotalProjects { get; set; }

        public int ActiveProjects { get; set; }

        public int ActiveAllocations { get; set; }

        public int PendingLeaves { get; set; }

        public int TodayAttendance { get; set; }

        public int EmployeesOnLeave { get; set; } = 0;
        // Employee metrics
        public int MyProjects { get; set; }

        public int MyPendingLeaves { get; set; }

        public int MyAttendanceCount { get; set; }

        public double MyAverageHours { get; set; }
    }
}
