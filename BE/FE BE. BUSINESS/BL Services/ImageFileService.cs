using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories;
using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Image = System.Drawing.Image;


namespace FE_BE._BUSINESS.BL_Services
{
    internal class ImageFileService : IImageFileService
    {
        private readonly ImageRepository _imageDBRepository;

        public ImageFileService(ImageRepository imageDBRepository)
        {
            _imageDBRepository = imageDBRepository;
        }

        // ====================== methods ======================
        public int AddImage(ImageFile imageFile)
        {
            _imageDBRepository.AddImage(imageFile);
            return imageFile.Id;
        }

        public ImageFile GetImage(int id)
        {
            return _imageDBRepository.GetImage(id);
        }
        // ============================ resize method =======================

        public ImageFile ResizeImage(ImageFile originalImageFile)
        {
            using (var originalImageStream = new MemoryStream(originalImageFile.Content))
            {
                var originalImage = Image.FromStream(originalImageStream);

                // Resize the image to 200x200 (stretched if necessary)
                var resizedWidth = 200;
                var resizedHeight = 200;

                using var resizedImage = new Bitmap(resizedWidth, resizedHeight);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(originalImage, 0, 0, resizedWidth, resizedHeight);
                }

                // Save the resized image to a MemoryStream
                using var resizedImageStream = new MemoryStream();
                resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Create a new ImageFile for the resized image
                var resizedImageFile = new ImageFile
                {
                    Content = resizedImageStream.ToArray(),
                    ContentType = "image/jpeg", // Set the content type accordingly
                    FileName = $"{Path.GetFileNameWithoutExtension(originalImageFile.FileName)}_resized.jpg", //File name 
                    Size = (int)resizedImageStream.Length
                };

                return resizedImageFile;
            }
        }


        // ============================ new methods =======================


    }
}
