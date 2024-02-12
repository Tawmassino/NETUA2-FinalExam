using FE_BE._DATA.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.API_Services;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.API.UnitTests
{
    public class UserMapperTests
    {
        [Fact]
        public void Map_CreateDTOToModel_ReturnsUserModel()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<UserMapper>>();

            var userMapper = new UserMapper(userServiceMock.Object, loggerMock.Object);
            var createDTO = new UserCreateDTO
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "testpassword"
            };

            // Act
            var userModel = userMapper.Map(createDTO);

            // Assert
            Assert.NotNull(userModel);
            Assert.Equal("testuser", userModel.Username);
            Assert.Equal("testuser@example.com", userModel.Email);
            // Password should not be set directly in the model
            Assert.Null(userModel.Password);
        }

        [Fact]
        public void Map_UpdateDTOToModel_ReturnsUserModel()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(us => us.CreatePasswordHash(It.IsAny<string>(), out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny));

            var loggerMock = new Mock<ILogger<UserMapper>>();

            var userMapper = new UserMapper(userServiceMock.Object, loggerMock.Object);
            var updateDTO = new UserUpdateDTO
            {
                Username = "updateduser",
                Email = "updateduser@example.com",
                Password = "updatedpassword",
                Role = "Admin"
            };

            // Act
            var userModel = userMapper.Map(updateDTO);

            // Assert
            Assert.NotNull(userModel);
            Assert.Equal("updateduser", userModel.Username);
            Assert.Equal("updateduser@example.com", userModel.Email);
            Assert.NotNull(userModel.Password);
            Assert.NotNull(userModel.PasswordSalt);
            Assert.Equal("Admin", userModel.Role);
        }


    }
}