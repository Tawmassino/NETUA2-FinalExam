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
using Microsoft.AspNetCore.Http;

namespace FinalExam.API.UnitTests
{
    public class UserServiceTests
    {
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
                null, // Mock IHttpContextAccessor? 
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
                
    }
}
