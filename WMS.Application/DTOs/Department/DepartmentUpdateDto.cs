using System;
using System.Collections.Generic;

namespace WMS.Application.DTOs.Department
{
    public class DepartmentUpdateDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
