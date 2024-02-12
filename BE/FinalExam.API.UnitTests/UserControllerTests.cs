using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.Controllers;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;

namespace FinalExam.API.UnitTests
{
    public class UnitTest1
    {

        // Mock dependencies
        private readonly Mock<ILogger<UserController>> _loggerMock = new Mock<ILogger<UserController>>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IJwtService> _jwtServiceMock = new Mock<IJwtService>();
        private readonly Mock<IUserDBRepository> _userDBRepositoryMock = new Mock<IUserDBRepository>();
        private readonly Mock<IPersonRepository> _personRepositoryMock = new Mock<IPersonRepository>();



        [Fact]
        public void Login_ValidUser_ReturnsOkObjectResult()
        {
            // Arrange
            var controller = new UserController(
                _loggerMock.Object,
                _userServiceMock.Object,
                _jwtServiceMock.Object,
                _userDBRepositoryMock.Object,
                _personRepositoryMock.Object);

            var userGetDto = new UserGetDTO
            {
                Username = "validUser",
                Password = "validPassword"
            };

            _userServiceMock.Setup(x => x.Login(userGetDto.Username, userGetDto.Password, out It.Ref<string>.IsAny))
                .Returns((true, new User())); // Assuming a valid user is returned

            _jwtServiceMock.Setup(x => x.GetJwtToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("validJwtToken");

            // Act
            var result = controller.Login(userGetDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Register_ValidUser_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var controller = new UserController(
                _loggerMock.Object,
                _userServiceMock.Object,
                _jwtServiceMock.Object,
                _userDBRepositoryMock.Object,
                _personRepositoryMock.Object);

            var userCreateDto = new UserCreateDTO
            {
                Username = "newUser",
                Password = "newPassword",
                Email = "newUser@example.com"
            };

            _userServiceMock.Setup(x => x.Register(userCreateDto.Username, userCreateDto.Password, userCreateDto.Email))
        .Returns(new UserResponse { IsSuccess = true, Message = "Registration successful" });

            // Act
            var result = controller.Register(userCreateDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public void Delete_ExistingUser_ReturnsNoContentResult()
        {
            // Arrange
            var controller = new UserController(
                _loggerMock.Object,
                _userServiceMock.Object,
                _jwtServiceMock.Object,
                _userDBRepositoryMock.Object,
                _personRepositoryMock.Object);

            var userId = 1;

            _userDBRepositoryMock.Setup(x => x.GetUserById(userId))
                .Returns(new User()); // Assuming a valid user exists

            // Act
            var result = controller.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}