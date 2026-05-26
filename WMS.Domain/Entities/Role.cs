using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        
        public string RoleName { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public ICollection<UserLogin> Users { get; set; } = new List<UserLogin>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
