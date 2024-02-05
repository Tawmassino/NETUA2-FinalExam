using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Interfaces
{
    public interface IImageRepository
    {
        ImageFile GetImage(int id);
        int AddImage(ImageFile image);
        void DeleteImage(int id);
    }
}
