using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._BUSINESS.BL_Services.BL_Interfaces
{
    public interface IImageFileService
    {
        ImageFile GetImage(int id);
        int AddImage(ImageFile imageFile);
        ImageFile ResizeImage(ImageFile originalImageFile);
    }
}
