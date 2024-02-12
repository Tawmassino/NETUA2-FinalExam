using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly FinalExamDbContext _imageDbContext;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(FinalExamDbContext imageRepository, ILogger<ImageRepository> logger)
        {
            _imageDbContext = imageRepository;
            _logger = logger;
        }

        // ==================== methods ====================
        public int AddImage(ImageFile image)
        {
            _imageDbContext.Images.Add(image);
            _imageDbContext.SaveChanges();
            _logger.LogInformation($"Image {image.FileName} (ID: {image.Id}) has been successfully added to the database ");
            return (int)image.Id;
        }

        public ImageFile GetImage(int id)
        {
            return _imageDbContext.Images.Find(id);
        }


        public void DeleteImage(int id)
        {
            var imageToDelete = _imageDbContext.Images.Find(id);
            if (imageToDelete != null)
            {
                _imageDbContext.Remove(imageToDelete);
                _imageDbContext.SaveChanges();
                _logger.LogInformation($"Image {imageToDelete.FileName} (ID: {imageToDelete.Id}) has been successfully deleted ");
            }

        }
    }
}
