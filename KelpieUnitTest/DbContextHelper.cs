using KelpieServer;
using KelpieServer.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Drawing.Text;

namespace KelpieUnitTest
{
    internal static class DbContextHelper
    {
        public static Mock<IDataContext> GetMockedDbContext()
        {
            var dbContextMock = new Mock<IDataContext>();

            return dbContextMock;
        }

        public static Mock<IDataContext> GetMockedDbContextWithUsers()
        {
            static List<User> GetMockUserList()
            {
                return new List<User>()
                {
                    new User
                    {
                        Id = 0,
                        Admin = false,
                        Username = "AlphaAlpha",
                        Password = "pw0",
                        Email = "emailalpha@mail.fake"
                    },
                    new User
                    {
                        Id = 1,
                        Admin = false,
                        Username = "BravoBravo",
                        Password = "pw1",
                        Email = "emailbravo@mail.fake"
                    },
                    new User
                    {
                        Id = 2,
                        Admin = false,
                        Username = "CharlieCharlie",
                        Password = "pw2",
                        Email = "emailcharlie@mail.fake"
                    },
                    new User
                    {
                        Id = 3,
                        Admin = false,
                        Username = "DeltaDelta",
                        Password = "pw3",
                        Email = "emaildelta@mail.fake"
                    }
                };
            }

            var dbContextMock = GetMockedDbContext();
            var users = GetMockUserList();
            dbContextMock.Setup<DbSet<User>>(o => o.Users).ReturnsDbSet(users);

            // set up mock for Users DbSet
            //var usersDbSetMock = new Mock<DbSet<User>>();

            // retrieve specific user if key provided
            //usersDbSetMock.Setup(o => o.FindAsync(It.IsAny<object[]>()))
            //    .ReturnsAsync((object[] keyValues) =>
            //    {
            //        var id = (int)keyValues[0];
            //        return users.SingleOrDefault(u => u.Id == id);
            //    });
            //dbContextMock.Setup(o => o.Users).Returns(usersDbSetMock.Object);

            return dbContextMock;
        }
    }
}
