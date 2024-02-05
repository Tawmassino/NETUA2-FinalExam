using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
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

        public ImageRepository(FinalExamDbContext imageRepository)
        {
            _imageDbContext = imageRepository;
        }

        // ==================== methods ====================
        public int AddImage(ImageFile image)
        {
            _imageDbContext.Images.Add(image);
            _imageDbContext.SaveChanges();
            return image.Id;
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
            }

        }
    }
}
