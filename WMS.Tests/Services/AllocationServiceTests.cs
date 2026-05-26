using Moq;
using WMS.Application.DTOs.Allocation;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class AllocationServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Allocation()
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

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                ProjectName = "Testing Project",
                ClientId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(2),
                Status = "Active"
            });

            await context.SaveChangesAsync();

            var auditMock = new Mock<IAuditService>();

            var service = new AllocationService(
                context,
                auditMock.Object
            );

            var dto = new AllocationCreateDto
            {
                EmpId = 1,
                ProjectId = 1,
                CreatedBy = "Admin"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.EmpId);
        }
    }
}