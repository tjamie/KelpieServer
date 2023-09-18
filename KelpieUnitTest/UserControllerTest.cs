using Moq;
using Moq.EntityFrameworkCore;
using KelpieServer.Controllers;
using KelpieServer.Models;
using Microsoft.AspNetCore.Mvc;
using KelpieServer;
using Microsoft.EntityFrameworkCore;

namespace KelpieUnitTest
{
    public class UserControllerTest
    {
        [Fact]
        public async Task GetUsers_ReturnsListOfUserDtos()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContextWithUsers();
            var controller = new UsersController(dbContext.Object);

            // Act
            var usersResponse = await controller.GetUsers();

            // Assert
            var result = usersResponse.Result as OkObjectResult;
            Assert.NotNull(result);

            var users = result.Value as IEnumerable<UserResponseDto>;
            Assert.Equal(4, users?.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsSingleUserDto()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContextWithUsers();
            var controller = new UsersController(dbContext.Object);
            //var usersDbSetMock = new Mock<DbSet<User>>();
            //usersDbSetMock.Setup(o => o.FindAsync(It.IsAny<object[]>()))
            //    .ReturnsAsync((object[] keyValues) =>
            //    {
            //        var id = (int)keyValues[0];
            //        return users.SingleOrDefault(u => u.Id == id);
            //    });

            // Act
            var userResponse = await controller.GetUser(3);

            // Assert
            var result = userResponse.Result as OkObjectResult;
            //Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            var user = result.Value as UserResponseDto;
            Assert.NotNull(user);
            Assert.Equal(3, user.Id);
            Assert.Equal("DeltaDelta", user.Username);
        }
    }
}