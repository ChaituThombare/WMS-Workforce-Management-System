using Moq;
using WMS.Application.DTOs.Leave;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;

namespace WMS.Tests.Services
{
    public class LeaveServiceTests
    {
        [Fact]
        public async Task ApproveLeave_Should_Update_Status()
        {
            var context = TestDbContextFactory.Create();

            context.LeaveRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 1,
                EmployeeId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                LeaveType = "Casual",
                Reason = "Testing",
                Status = "Pending",
                AppliedOn = DateTime.Now
            });

            await context.SaveChangesAsync();

            var auditMock = new Mock<IAuditService>();

            var service = new LeaveService(
                context,
                auditMock.Object
            );

            await service.ApproveAsync(
                1,
                new LeaveDecisionDto
                {
                    ManagerComments = "Approved",
                    ApprovedBy = "Admin"
                }
            );

            var leave = await context.LeaveRequests.FindAsync(1);

            Assert.Equal("Approved", leave!.Status);
        }
    }
}