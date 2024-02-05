using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.DTOs;
using System.Net.Mime;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing;




// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NETUA2_FinalExam_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IImageRepository _imageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDBRepository _userDBRepository;

        public ImageController(
            ILogger<ImageController> logger,
            IImageRepository imageRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserDBRepository userDBRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
            _httpContextAccessor = httpContextAccessor;
            _userDBRepository = userDBRepository;
        }

        // ==================== methods ====================
        // GET: api/<ImageController>
        [HttpGet]
        //[ProducesResponseType(typeof(List<ImageResultDto>), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // POST api/<ImageController>
        [HttpPost("UploadImage")]
        public IActionResult UploadImage(ImageUploadRequest request)
        {
            using var memorystream = new MemoryStream();
            request.Image.CopyTo(memorystream);//DTO
            var imageBytes = memorystream.ToArray();
            var imageFile = new ImageFile
            {
                Content = imageBytes,
                ContentType = request.Image.ContentType,
                FileName = request.Image.FileName,
                Size = imageBytes.Length
            };

            return Ok(_imageRepository.AddImage(imageFile));
        }


        [HttpGet("download/{id}")]
        public IActionResult Download(int id)
        {

            var imageFile = _imageRepository.GetImage(id);
            if (imageFile == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Downloading file with id: {id}");
            return File(imageFile.Content, imageFile.ContentType, imageFile.FileName);
        }


        [HttpPost("upload and create thumbnail")]
        public IActionResult UploadImageAndCreateThumbnail([FromForm] ImageUploadRequest request)
        {
            using var memoryStream = new MemoryStream();
            request.Image.CopyTo(memoryStream); //  DTO  property Image of type IFormFile

            var originalImage = Image.FromStream(memoryStream);

            // Create a thumbnail
            var thumbnailWidth = 100; //set width
            var thumbnailHeight = (int)((double)originalImage.Height / originalImage.Width * thumbnailWidth);

            using var thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);
            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
            }

            // Save the thumbnail to a MemoryStream
            using var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Create an ImageFile for the thumbnail
            var thumbnailBytes = thumbnailStream.ToArray();

            var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(request.Image.FileName).Split('.').First()}_thumbnail.jpg";

            var thumbnailFile = new ImageFile
            {
                Content = thumbnailBytes,
                ContentType = "image/jpeg", // Set the content type accordingly
                FileName = thumbnailFileName, // Set the file name accordingly
                Size = thumbnailBytes.Length
            };

            // Add the original image and thumbnail to the ImageFileService
            var imageBytes = memoryStream.ToArray();
            var originalImageFile = new ImageFile
            {
                Content = imageBytes,
                ContentType = request.Image.ContentType,
                FileName = request.Image.FileName,
                Size = imageBytes.Length
            };

            // Use the GetImageFile method to get the ImageFile after adding it to the repository
            var addedOriginalImageFile = _imageRepository.GetImage(_imageRepository.AddImage(originalImageFile));
            var addedThumbnailFile = _imageRepository.GetImage(_imageRepository.AddImage(thumbnailFile));

            var addedFiles = new List<ImageFile>
            {
            addedOriginalImageFile,
            addedThumbnailFile
            };

            return Ok(addedFiles);
        }

    }
}
