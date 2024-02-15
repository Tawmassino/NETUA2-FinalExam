using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.DTOs;
using System.Net.Mime;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using FE_BE._DATA.DB_Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using System.Security.Claims;




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
        private readonly IImageFileService _imageFileService;
        private readonly IPersonRepository _personRepository;
        private ILogger<ImageController> object1;
        private IImageRepository object2;
        private IHttpContextAccessor object3;
        private IUserDBRepository object4;
        private IImageFileService object5;

        public ImageController(
            ILogger<ImageController> logger,
            IImageRepository imageRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserDBRepository userDBRepository,
            IImageFileService imageFileService,
            IPersonRepository personRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
            _httpContextAccessor = httpContextAccessor;
            _userDBRepository = userDBRepository;
            _imageFileService = imageFileService;
            _personRepository = personRepository;
        }

        public ImageController(ILogger<ImageController> object1, IImageRepository object2, IHttpContextAccessor object3, IUserDBRepository object4, IImageFileService object5)
        {
            this.object1 = object1;
            this.object2 = object2;
            this.object3 = object3;
            this.object4 = object4;
            this.object5 = object5;
        }

        // ==================== methods ====================
        // GET: api/<ImageController>
        [HttpGet("GetImage")]
        //[ProducesResponseType(typeof(List<ImageResultDto>), StatusCodes.Status200OK)]
        //[Produces(MediaTypeNames.Application.Json)]
        public IActionResult GetImage([FromRoute] int imageId)
        {
            var imageToGet = _imageFileService.GetImage(imageId);
            return Ok(imageToGet.Content);
        }



        [HttpPost("UploadImage")]
        public IActionResult UploadImage(ImageUploadRequest request)
        {
            try
            {
                // Convert the uploaded image to bytes
                using var memoryStream = new MemoryStream();
                request.Image.CopyTo(memoryStream); // DTO property Image (type IFormFile)
                var imageBytes = memoryStream.ToArray();

                // Create ImageFile from the uploaded image
                var imageFile = new ImageFile
                {
                    Content = imageBytes,
                    ContentType = request.Image.ContentType,
                    FileName = request.Image.FileName,
                    Size = imageBytes.Length
                };

                // Resize the image and add it to the repository
                var resizedImageFile = _imageFileService.ResizeImage(imageFile);
                var addedResizedImageFile = _imageFileService.GetImage(_imageFileService.AddImage(resizedImageFile));

                //update person image id
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var personToUpdate = _personRepository.GetPersonByUserId(int.Parse(userId));
                personToUpdate.ProfilePictureId = addedResizedImageFile.Id;
                _personRepository.UpdatePerson(personToUpdate);

                return Ok(addedResizedImageFile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("download/{id}")]
        public IActionResult Download(int id)
        {
            // Get the image file by ID from the repository
            var imageFile = _imageRepository.GetImage(id);
            if (imageFile == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Downloading file with id: {id}");
            return File(imageFile.Content, imageFile.ContentType, imageFile.FileName);
        }

        //------------------------------------

        [HttpPost("upload and create thumbnail")]
        public IActionResult UploadImageAndCreateThumbnail([FromForm] ImageUploadRequest request)
        {
            using var memoryStream = new MemoryStream();
            request.Image.CopyTo(memoryStream); //  DTO  property Image of type IFormFile

            var originalImage = Image.FromStream(memoryStream);

            // Create a thumbnail
            var thumbnailWidth = 200; //set width
            var thumbnailHeight = (int)((double)originalImage.Height / originalImage.Width * thumbnailWidth);

            using var thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);
            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
            }

            // Save the thumbnail to a MemoryStream
            using var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            //Create an ImageFile for the thumbnail
            var thumbnailBytes = thumbnailStream.ToArray();

            var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(request.Image.FileName).Split('.').First()}_thumbnail.jpg";

            var thumbnailFile = new ImageFile
            {
                Content = thumbnailBytes,
                ContentType = "image/jpeg", //Set the content type 
                FileName = thumbnailFileName, //Set the file name 
                Size = thumbnailBytes.Length
            };

            //Add the original image and thumbnail
            var imageBytes = memoryStream.ToArray();
            var originalImageFile = new ImageFile
            {
                Content = imageBytes,
                ContentType = request.Image.ContentType,
                FileName = request.Image.FileName,
                Size = imageBytes.Length
            };

            //Use the GetImageFile method to get the ImageFile after adding it to the repository
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

// ==================== obsolete ====================


//[HttpPost("UploadImage")]
//public IActionResult UploadImage(ImageUploadRequest request)
//{
//    using var memorystream = new MemoryStream();
//    request.Image.CopyTo(memorystream);//DTO
//    var imageBytes = memorystream.ToArray();
//    var imageFile = new ImageFile
//    {
//        Content = imageBytes,
//        ContentType = request.Image.ContentType,
//        FileName = request.Image.FileName,
//        Size = imageBytes.Length
//    };

//    return Ok(_imageRepository.AddImage(imageFile));
//}





//[HttpPost("upload and create thumbnail")]
//public IActionResult UploadImageAndCreateThumbnail([FromForm] ImageUploadRequest request)
//{
//    using var memoryStream = new MemoryStream();
//    request.Image.CopyTo(memoryStream); //  DTO  property Image of type IFormFile

//    var originalImage = Image.FromStream(memoryStream);

//    // Create a thumbnail
//    var thumbnailWidth = 200; //set width
//    var thumbnailHeight = (int)((double)originalImage.Height / originalImage.Width * thumbnailWidth);

//    using var thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);
//    using (var graphics = Graphics.FromImage(thumbnail))
//    {
//        graphics.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
//    }

//    // Save the thumbnail to a MemoryStream
//    using var thumbnailStream = new MemoryStream();
//    thumbnail.Save(thumbnailStream, System.Drawing.Imaging.ImageFormat.Jpeg);

//    //Create an ImageFile for the thumbnail
//    var thumbnailBytes = thumbnailStream.ToArray();

//    var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(request.Image.FileName).Split('.').First()}_thumbnail.jpg";

//    var thumbnailFile = new ImageFile
//    {
//        Content = thumbnailBytes,
//        ContentType = "image/jpeg", //Set the content type 
//        FileName = thumbnailFileName, //Set the file name 
//        Size = thumbnailBytes.Length
//    };

//    //Add the original image and thumbnail
//    var imageBytes = memoryStream.ToArray();
//    var originalImageFile = new ImageFile
//    {
//        Content = imageBytes,
//        ContentType = request.Image.ContentType,
//        FileName = request.Image.FileName,
//        Size = imageBytes.Length
//    };

//    //Use the GetImageFile method to get the ImageFile after adding it to the repository
//    var addedOriginalImageFile = _imageRepository.GetImage(_imageRepository.AddImage(originalImageFile));
//    var addedThumbnailFile = _imageRepository.GetImage(_imageRepository.AddImage(thumbnailFile));
//    var addedFiles = new List<ImageFile>
//            {
//                    addedOriginalImageFile,
//                    addedThumbnailFile
//                    };

//    return Ok(addedFiles);
//}
//    }