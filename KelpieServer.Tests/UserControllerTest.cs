using Moq;
using Moq.EntityFrameworkCore;
using KelpieServer.Controllers;
using KelpieServer.Models;
using Microsoft.AspNetCore.Mvc;
using KelpieServer;
using Microsoft.EntityFrameworkCore;

namespace KelpieServer.Tests
{
    public class UserControllerTest
    {
        // HTTP GET - all users
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

        // HTTP GET - single user
        [Fact]
        public async Task GetUser_ReturnsSingleUserDto()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContextWithUsers();
            var controller = new UsersController(dbContext.Object);
            var users = DbContextHelper.GetMockUserList();
            var usersDbSetMock = new Mock<DbSet<User>>();
            // Replace FindAsync() from controller
            usersDbSetMock.Setup(o => o.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] keyValues) =>
                {
                    var id = (int)keyValues[0];
                    return users.SingleOrDefault(u => u.Id == id);
                });
            dbContext.Setup(o => o.Users).Returns(usersDbSetMock.Object);

            // Act
            var userResponse = await controller.GetUser(3);

            // Assert
            var result = userResponse.Result as OkObjectResult;
            Assert.NotNull(result);
            var user = result.Value as UserResponseDto;
            Assert.NotNull(user);
            Assert.Equal(3, user.Id);
            Assert.Equal("DeltaDelta", user.Username);
        }

        // HTTP PUT
        [Fact]
        public async Task PutUser_ReturnsUserDto()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContextWithUsers();
            var controller = new UsersController(dbContext.Object);
            var users = DbContextHelper.GetMockUserList();
            var usersDbSetMock = new Mock<DbSet<User>>();
            // Replace FindAsync() from controller
            usersDbSetMock.Setup(o => o.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] keyValues) =>
                {
                    var id = (int)keyValues[0];
                    return users.SingleOrDefault(u => u.Id == id);
                });
            dbContext.Setup(o => o.Users).Returns(usersDbSetMock.Object);
            var updatedUserDto = new UserDto()
            {
                Id = 3,
                Admin = false,
                Username = "TestNameDelta",
                Password = "testpassword",
                Email = "TestEmailDelta@fake.mail"
            };

            // Act
            var userPutResponse = await controller.PutUser(3, updatedUserDto);

            // Assert
            var result = userPutResponse.Result as OkObjectResult;
            Assert.NotNull(result);
            var modifiedUser = result.Value as UserResponseDto;
            Assert.NotNull(modifiedUser);
            Assert.Equal(3, modifiedUser.Id);
            Assert.Equal("TestNameDelta", modifiedUser.Username);
        }

        // HTTP POST
        [Fact]
        public async Task PostUser_ReturnsUser()
        {
            // Arrange
            var dbContext = DbContextHelper.GetMockedDbContextWithUsers();
            var controller = new UsersController(dbContext.Object);
            var newUserDto = new UserDto()
            {
                Id = 4,
                Admin = false,
                Username = "User5",
                Password = "testpassword",
                Email = "user5@fake.mail"
            };

            // Act
            var userPostResponse = await controller.PostUser(newUserDto);

            // Assert
            var result = userPostResponse.Result as CreatedAtActionResult;
            Assert.NotNull(result);
            var user = result.Value as User;
            Assert.NotNull(user);
            Assert.False(user.Admin);
            Assert.Equal("User5", user.Username);
            Assert.Equal("user5@fake.mail", user.Email);
        }
    }
}