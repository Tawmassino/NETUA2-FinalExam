using FE_BE._DATA.Entities;
using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs
{
    public class PersonCreateDTO
    {
        //[Required(ErrorMessage = "Surname is required.")]
        //[StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        public string Name { get; set; }



        //[StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters.")]
        public string? Surname { get; set; }



        //[RegularExpression(@"^\d{11}$", ErrorMessage = "SocialSecurityNumber must be exactly 11 numeric characters.")]
        //[StringLength(11, ErrorMessage = "SocialSecurityNumber must be exactly 11 characters.")]
        public string? SocialSecurityNumber { get; set; }

        //[RegularExpression(@"^\d{11}$", ErrorMessage = "Phonenumber must be exactly 12 numeric characters.")]
        //[StringLength(12, ErrorMessage = "Phonenumber must be exactly 12 characters.")]
        public string? PhoneNumber { get; set; }


        //[EmailAddress]
        public string? Email { get; set; }




        //kaip gauti situos? ar ju reikia?
        public int UserId { get; set; }
        public int? UserLocationId { get; set; }
        //public IFormFile? ProfilePicture { get; set; }
    }
}
