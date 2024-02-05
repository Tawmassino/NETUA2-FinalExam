using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Repositories;
using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._BUSINESS.BL_Services
{
    internal class ImageFileService : IImageFileService
    {
        private readonly ImageRepository _imageDBRepository;

        public ImageFileService(ImageRepository imageDBRepository)
        {
            _imageDBRepository = imageDBRepository;
        }

        public int AddImage(ImageFile imageFile)
        {
            return _imageDBRepository.AddImage(imageFile);
        }

        public ImageFile GetImage(int id)
        {
            return _imageDBRepository.GetImage(id);
        }
    }
}
