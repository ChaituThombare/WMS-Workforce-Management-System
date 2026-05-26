using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Domain.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public DateTime DOJ { get; set; }

        public int DepartmentId { get; set; }

        public int RoleId { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }

        public Department Department { get; set; } = null!;

        public Role Role { get; set; } = null!;
        
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        public ICollection<EmployeeProjectAllocation> ProjectAllocations { get; set; } = new List<EmployeeProjectAllocation>();
    }
}
