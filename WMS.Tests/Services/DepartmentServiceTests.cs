using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using WMS.Application.DTOs.Department;
using Xunit;
using Moq;
using WMS.Application.Interfaces;

namespace WMS.Tests.Services
{
    public class DepartmentServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Department()
        {
            var context = TestDbContextFactory.Create();

            var auditMock = new Mock<IAuditService>();

            var service = new DepartmentService(
                context,
                auditMock.Object
            );

            var dto = new DepartmentCreateDto
            {
                DepartmentName = "Testing Department",
                Description = "Unit Test"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Testing Department", result.DepartmentName);
        }
    }
}