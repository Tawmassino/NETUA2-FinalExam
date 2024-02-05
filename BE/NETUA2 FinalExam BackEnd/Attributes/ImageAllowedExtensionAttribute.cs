using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.Attributes
{
    public class ImageAllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedImageExtensions;

        public ImageAllowedExtensionAttribute(string[] allowedImageExtensions)
        {
            _allowedImageExtensions = allowedImageExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is FormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_allowedImageExtensions.Contains(extension))
                {
                    return new ValidationResult($"File extension {extension} is not allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
