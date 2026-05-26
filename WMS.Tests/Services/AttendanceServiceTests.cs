using WMS.Application.DTOs.Attendance;
using WMS.Domain.Entities;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class AttendanceServiceTests
    {
        [Fact]
        public async Task CheckInAsync_Should_Create_Attendance_Record()
        {
            var context = TestDbContextFactory.Create();

            context.Employees.Add(new Employee
            {
                EmployeeId = 1,
                FirstName = "Rahul",
                LastName = "Patil",
                Email = "rahul@test.com",
                PhoneNumber = "9999999999",
                Gender = "M",
                DOB = DateTime.Today.AddYears(-25),
                DOJ = DateTime.Today,
                DepartmentId = 1,
                RoleId = 2,
                Status = "Active",
                CreatedOn = DateTime.Now
            });

            await context.SaveChangesAsync();

            var service = new AttendanceService(context);

            var dto = new AttendanceCheckInDto
            {
                EmployeeId = 1,
                WorkMode = "Office"
            };

            await service.CheckInAsync(dto);

            Assert.Single(context.Attendances);
        }
    }
}