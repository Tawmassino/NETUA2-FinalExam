using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NETUA2_FinalExam_BackEnd.Controllers;
using NETUA2_FinalExam_BackEnd.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.API.UnitTests
{
    public class ImageControllerTests
    {
        private readonly Mock<ILogger<ImageController>> _loggerMock = new Mock<ILogger<ImageController>>();
        private readonly Mock<IImageRepository> _imageRepositoryMock = new Mock<IImageRepository>();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IUserDBRepository> _userDBRepositoryMock = new Mock<IUserDBRepository>();
        private readonly Mock<IImageFileService> _imageFileServiceMock = new Mock<IImageFileService>();

        [Fact]
        public void GetImage_ValidImageId_ReturnsOkResult()
        {
            // Arrange
            var imageController = new ImageController(
                _loggerMock.Object,
                _imageRepositoryMock.Object,
                _httpContextAccessorMock.Object,
                _userDBRepositoryMock.Object,
                _imageFileServiceMock.Object);

            var imageId = 1;
            _imageFileServiceMock.Setup(x => x.GetImage(imageId))
                .Returns(new ImageFile()); // Mocking a valid image file

            // Act
            var result = imageController.GetImage(imageId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void UploadImage_ValidImage_ReturnsOkResult()
        {
            // Arrange
            var imageController = new ImageController(
                _loggerMock.Object,
                _imageRepositoryMock.Object,
                _httpContextAccessorMock.Object,
                _userDBRepositoryMock.Object,
                _imageFileServiceMock.Object);

            var imageUploadRequest = new ImageUploadRequest
            {
                Image = new FormFile(Stream.Null, 0, 0, "image", "image.jpg")
            };

            _imageFileServiceMock.Setup(x => x.ResizeImage(It.IsAny<ImageFile>()))
                .Returns(new ImageFile()); // Mocking a resized image file

            // Act
            var result = imageController.UploadImage(imageUploadRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }


    }
}
