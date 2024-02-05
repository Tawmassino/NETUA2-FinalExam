using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class UserGetDTO
    {
        //getDTOs dont require validators

        public int Id { get; set; } //id parameter in DTO is required only in get method
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
