using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.Attributes
{
    public class ImageMaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public ImageMaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        // ============= method =============

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxFileSize)
            {
                return new ValidationResult($"The file size cannot exceed {_maxFileSize}");
            }
            return ValidationResult.Success;
        }
    }
}
