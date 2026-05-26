using WMS.Application.DTOs.Auth;
using WMS.Domain.Entities;
using WMS.Infrastructure.Identity;
using WMS.Infrastructure.Services;
using WMS.Tests.Helpers;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WMS.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task LoginAsync_Should_Return_Token_For_Valid_User()
        {
            var context = TestDbContextFactory.Create();

            context.Roles.Add(new Role
            {
                RoleId = 1,
                RoleName = "Admin",
                Description = "Admin"
            });

            context.UserLogins.Add(new UserLogin
            {
                Username = "admin",
                PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                RoleId = 1
            });

            await context.SaveChangesAsync();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsASuperSecretKeyForTesting12345" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" }
                })
                .Build();

            var jwtService = new JwtTokenService(config);

            var service = new AuthService(
                context,
                jwtService
            );

            var request = new LoginRequestDto
            {
                Username = "admin",
                Password = "Admin@123"
            };

            var result = await service.LoginAsync(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }
    }
}