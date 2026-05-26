using Moq;
using WMS.Application.DTOs.Client;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Client()
        {
            var context = TestDbContextFactory.Create();

            var auditMock = new Mock<IAuditService>();

            var service = new ClientService(
                context,
                auditMock.Object
            );

            var dto = new ClientCreateDto
            {
                ClientName = "Infosys",
                ClientAddress = "Pune",
                ClientPhoneNumber = "9876543210",
                ClientLocation = "India",
                Status = true
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Infosys", result.ClientName);
        }
    }
}