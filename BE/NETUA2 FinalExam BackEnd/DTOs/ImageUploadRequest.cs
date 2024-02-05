using NETUA2_FinalExam_BackEnd.Attributes;

namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class ImageUploadRequest
    {
        //Atributes
        [ImageMaxFileSize(5 * 1024 * 1024)] // 5MB - 5 bytes * kilobyte * megabyte
        [ImageAllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".bmp", })]
        public IFormFile Image { get; set; }
    }
}
