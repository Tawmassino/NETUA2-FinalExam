using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.Attributes
{
    public class PasswordValidatorAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 4;
        public bool RequireUppercase { get; set; } = false;
        public bool RequireLowercase { get; set; } = false;
        public bool RequireDigit { get; set; } = false;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Password is required.");
            }

            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult($"Password must be at least {MinimumLength} characters long.");
            }

            if (RequireUppercase && !password.Any(char.IsUpper))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            if (RequireDigit && !password.Any(char.IsDigit))
            {
                return new ValidationResult("Password must contain at least one number.");
            }

            return ValidationResult.Success;
        }

    }
}
