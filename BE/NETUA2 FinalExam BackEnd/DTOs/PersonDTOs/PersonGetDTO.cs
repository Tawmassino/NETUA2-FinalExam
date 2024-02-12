using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs
{
    public class PersonGetDTO
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress] public string? Email { get; set; }

        public int UserId { get; set; }

        public int? UserLocationId { get; set; }
        public int? ProfilePictureId { get; set; }
    }
}
