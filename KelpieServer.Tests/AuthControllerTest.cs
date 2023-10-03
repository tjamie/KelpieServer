using Moq;
using KelpieServer.Controllers;
using KelpieServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KelpieServer.Tests
{
    public class AuthControllerTest
    {
        [Fact]
        public void GenerateToken_ReturnsJwtSecurityToken()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContext();
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "testkeyabcdefghijklmnopqrstuvwxyz0123456789"},
                {"Jwt:Issuer", "localhost_test"},
                {"Jwt:Audience", "localhost_test"}
            };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var controller = new AuthController(configuration, dbContext.Object);

            UserDto user = new UserDto
            {
                Id = 197000804,
                Admin = false,
                Username = "test_user",
                Password = "test_password",
                Email = null
            };

            // Act
            var token = controller.GenerateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadJwtToken(token);
            var tokenId = readToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var tokenUsername = readToken.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            // Assert
            Assert.NotNull(token);
            Assert.Equal(user.Id.ToString(), tokenId);
            Assert.Equal(user.Username, tokenUsername);
        }
    }
}
