using FE_BE._DATA.Entities;
using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs
{
    public class PersonCreateDTO
    {
        [Required(ErrorMessage = "surname is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "name must be between 3 and 30 characters.")]
        public string Name { get; set; }



        [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters.")]
        public string? Surname { get; set; }



        //[RegularExpression(@"^\d{11}$", ErrorMessage = "SocialSecurityNumber must be exactly 11 numeric characters.")]
        [StringLength(11, ErrorMessage = "SocialSecurityNumber must be exactly 11 characters.")]
        public string? SocialSecurityNumber { get; set; }

        //[RegularExpression(@"^\d{11}$", ErrorMessage = "Phonenumber must be exactly 12 numeric characters.")]
        [StringLength(9, ErrorMessage = "Phonenumber must be exactly 9 characters.")]
        public string? PhoneNumber { get; set; }


        [EmailAddress]
        public string? Email { get; set; }





        public int UserId { get; set; }
        public int? UserLocationId { get; set; }
        //public IFormFile? ProfilePicture { get; set; }
    }
}
