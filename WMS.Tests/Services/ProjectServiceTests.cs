using Moq;
using WMS.Application.DTOs.Project;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Project()
        {
            var context = TestDbContextFactory.Create();

            context.Clients.Add(new Client
            {
                ClientId = 1,
                ClientName = "Test Client",
                ClientAddress = "Nagpur",
                ClientPhoneNumber = "9999999999",
                ClientLocation = "India",
                Status = true
            });

            await context.SaveChangesAsync();

            var auditMock = new Mock<IAuditService>();

            var service = new ProjectService(
                context,
                auditMock.Object
            );

            var dto = new ProjectCreateDto
            {
                ProjectName = "Payroll System",
                ClientId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3),
                Status = "Active"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Payroll System", result.ProjectName);
        }
    }
}