using Moq;
using WMS.Application.DTOs.Employee;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Employee()
        {
            var context = TestDbContextFactory.Create();

            context.Departments.Add(new Department
            {
                DepartmentId = 1,
                DepartmentName = "IT",
                Description = "Testing",
                CreatedOn = DateTime.Now
            });

            context.Roles.Add(new Role
            {
                RoleId = 2,
                RoleName = "Employee",
                Description = "Regular Employee"
            });

            await context.SaveChangesAsync();

            var auditMock = new Mock<IAuditService>();

            var service = new EmployeeService(
                context,
                auditMock.Object
            );

            var dto = new EmployeeCreateDto
            {
                FirstName = "Rahul",
                LastName = "Patil",
                Email = "rahul@test.com",
                PhoneNumber = "9876543210",
                Gender = "M",
                DOB = new DateTime(2000, 1, 1),
                DOJ = DateTime.Now,
                DepartmentId = 1,
                RoleId = 2,
                Password = "Patil@123"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Rahul", result.FirstName);
        }
    }
}