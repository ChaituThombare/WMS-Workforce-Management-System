using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Attendance
{
    public class AttendanceResponseDto
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }
        
        public string EmployeeName { get; set; } = string.Empty;

        public DateTime AttendanceDate { get; set; }
        
        public DateTime CheckInTime { get; set; }
        
        public DateTime? CheckOutTime { get; set; }
        
        public double TotalHours { get; set; }

        public string WorkMode { get; set; } = string.Empty;
    }
}
