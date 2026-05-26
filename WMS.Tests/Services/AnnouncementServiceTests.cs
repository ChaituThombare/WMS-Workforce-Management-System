using Moq;
using WMS.Application.DTOs.Announcement;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class AnnouncementServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Create_Announcement()
        {
            var context = TestDbContextFactory.Create();

            var auditMock = new Mock<IAuditService>();

            var service = new AnnouncementService(
                context,
                auditMock.Object
            );

            var dto = new CreateAnnouncementDto
            {
                Title = "System Update",
                Message = "Testing announcement",
                CreatedBy = "Admin"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("System Update", result.Title);
        }
    }
}