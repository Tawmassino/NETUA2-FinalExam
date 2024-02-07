using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs
{
    public class PersonUpdateDTO
    {
        [Required][StringLength(30, MinimumLength = 3)] public string Name { get; set; }
        [Required][StringLength(30, MinimumLength = 3)] public string? Surname { get; set; }
        [Required][StringLength(30, MinimumLength = 3)] public string? SocialSecurityNumber { get; set; }
        [Required][StringLength(30, MinimumLength = 3)] public string? PhoneNumber { get; set; }
        [EmailAddress] public string? Email { get; set; }

        public int UserId { get; set; }

        public int? ProfilePictureId { get; set; }



        //SITAS TURI SIETIS SU LOCATION, BET VENGTI CROSS-REFERENCE
        [ForeignKey(nameof(UserId))]
        public int? UserLocationId { get; set; }


        
    }
}
