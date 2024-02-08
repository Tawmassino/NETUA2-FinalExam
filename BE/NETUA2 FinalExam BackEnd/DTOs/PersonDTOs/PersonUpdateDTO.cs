using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs
{
    public class PersonUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Name cannot be empty or whitespace.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Surname is required.")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Surname cannot be empty or whitespace.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters.")]
        public string? Surname { get; set; }


        [Required(ErrorMessage = "Social Security Number is required.")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Name cannot be empty or whitespace.")]
        [StringLength(11, ErrorMessage = "Social Security Number must be 11 characters.")]
        public string? SocialSecurityNumber { get; set; }


        [Required]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Phone Number cannot be empty or whitespace.")]
        [StringLength(7, ErrorMessage = "Phone Number must be 7 characters (exluding: +370).")]
        public string? PhoneNumber { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Email cannot be empty or whitespace.Must follow order - recipient@address.domain")]
        public string? Email { get; set; }

        public int UserId { get; set; }


        public int? ProfilePictureId { get; set; }



        //SITAS TURI SIETIS SU LOCATION, BET VENGTI CROSS-REFERENCE
        [ForeignKey(nameof(UserId))]
        public int? UserLocationId { get; set; }



    }
}
