using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Attendance
{
    public class AttendanceCheckInDto
    {
        public int EmployeeId { get; set; }

        public string WorkMode { get; set; } = "Office";
    }
}
