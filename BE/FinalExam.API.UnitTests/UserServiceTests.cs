using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.API_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FinalExam.API.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var expectedPassword = "password"; // Password used in the test
            var userDBRepositoryMock = new Mock<IUserDBRepository>();
            userDBRepositoryMock.Setup(repo => repo.GetUserByUsername("testuser"))
                .Returns(new User
                {
                    Id = 1,
                    Username = "testuser",
                    Password = new byte[64], // Mocked password hash
                    PasswordSalt = new byte[128], // Mocked password salt
                    Role = "User"
                });

            var loggerMock = new Mock<ILogger<UserService>>();
            var jwtServiceMock = new Mock<IJwtService>();
            var personRepositoryMock = new Mock<IPersonRepository>();
            var locationRepositoryMock = new Mock<ILivingLocationRepository>();

            var userService = new UserService(
                null, // Mock IHttpContextAccessor as needed
                userDBRepositoryMock.Object,
                loggerMock.Object,
                jwtServiceMock.Object,
                personRepositoryMock.Object,
                locationRepositoryMock.Object);

            // Mock the non-public method VerifyPasswordHash on UserService
            var userServiceType = userService.GetType();
            var verifyPasswordHashMethod = userServiceType.GetMethod("VerifyPasswordHash", BindingFlags.NonPublic | BindingFlags.Instance);
            var verifyPasswordHashMock = new Mock<IUserService>();

            // Setup the mocked method to return true
            verifyPasswordHashMock.Setup(vph => vph.VerifyPasswordHash(expectedPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true); // or any other appropriate setup for your test

            // Act
            var (success, user) = userService.Login("testuser", expectedPassword, out _);

            // Assert
            Assert.True(success, "Login should be successful");
            Assert.NotNull(user);
            Assert.Equal("User", user.Role);

            // Validate that GetUserByUsername was called with the expected username
            userDBRepositoryMock.Verify(repo => repo.GetUserByUsername("testuser"), Times.Once);

            // Validate that VerifyPasswordHash was called on UserService
            verifyPasswordHashMock.Verify(vph => vph.VerifyPasswordHash(expectedPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }


        [Fact]
        public void Register_NewUser_ReturnsSuccessResponse()
        {
            // Arrange
            var userDBRepositoryMock = new Mock<IUserDBRepository>();
            userDBRepositoryMock.Setup(repo => repo.GetUserByUsername("newuser"))
                               .Returns((User)null); // No existing user with the same username

            userDBRepositoryMock.Setup(repo => repo.AddNewUser(It.IsAny<User>()))
                               .Returns(1); // Mocked user ID

            var loggerMock = new Mock<ILogger<UserService>>();
            var jwtServiceMock = new Mock<IJwtService>();
            var personRepositoryMock = new Mock<IPersonRepository>();
            var locationRepositoryMock = new Mock<ILivingLocationRepository>();

            var userService = new UserService(
                null, // Mock IHttpContextAccessor as needed
                userDBRepositoryMock.Object,
                loggerMock.Object,
                jwtServiceMock.Object,
                personRepositoryMock.Object,
                locationRepositoryMock.Object);

            // Act
            var response = userService.Register("newuser", "newpassword", "newuser@example.com");

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal("User newuser has been created successfully with ID: 1", response.Message);
        }

        // Additional tests for other methods can be added similarly
    }
}
