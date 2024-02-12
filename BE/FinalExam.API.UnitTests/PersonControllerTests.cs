using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.API.UnitTests
{
    public class PersonControllerTests
    {
        //[Fact]
        //public void GetPersonInfoFromUser_ReturnsPerson()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var userServiceMock = new Mock<IUserService>();
        //    userServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId);

        //    var loggerMock = new Mock<ILogger<PersonController>>();
        //    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        //    var personRepositoryMock = new Mock<IPersonRepository>();
        //    var personMapperMock = new Mock<IPersonMapper>();
        //    var imageFileServiceMock = new Mock<IImageFileService>();

        //    //su httpContextAccessorMock
        //    //taip: userServiceMock.Setup(service => service.GetCurrentUserId()).Returns(personId);


        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId.ToString() ) ,
        //        new Claim(ClaimTypes.Name, "Test"),
        //        new Claim(ClaimTypes.Email, "test@example.com")

        //    };
        //    var identity = new ClaimsIdentity(claims);
        //    var principal = new ClaimsPrincipal(identity);


        //    httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new Mock<HttpContext>().Object);
        //    httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(principal);

        //    var sut = new PersonController(
        //        loggerMock.Object,
        //        httpContextAccessorMock.Object,
        //        userServiceMock.Object,
        //        personRepositoryMock.Object,
        //        personMapperMock.Object,
        //        imageFileServiceMock.Object
        //    );

        //    //nera setup su var imageFileServiceMock = new Mock<IImageFileService>();

        //    // Act
        //    var result = sut.GetPersonInfoFromUser();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var person = Assert.IsType<Person>(okResult.Value);
        //    Assert.Equal(userId, person.UserId);
        //}

        [Fact]
        public void DeletePerson_ReturnsNoContent()
        {
            // Arrange
            var personId = 1;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetCurrentUserId()).Returns(personId);

            var loggerMock = new Mock<ILogger<PersonController>>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var personRepositoryMock = new Mock<IPersonRepository>();
            var personMapperMock = new Mock<IPersonMapper>();
            var imageFileServiceMock = new Mock<IImageFileService>();

            var sut = new PersonController(
                loggerMock.Object,
                httpContextAccessorMock.Object,
                userServiceMock.Object,
                personRepositoryMock.Object,
                personMapperMock.Object,
                imageFileServiceMock.Object
            );

            // Act
            var result = sut.DeletePerson(personId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            personRepositoryMock.Verify(repo => repo.DeletePersonById(personId), Times.Once);
        }
    }

}
