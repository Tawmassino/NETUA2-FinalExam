using NETUA2_FinalExam_BackEnd.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class UserCreateDTO
    {
        [Required][StringLength(30, MinimumLength = 3)] public string Username { get; set; }
        [EmailAddress] public string? Email { get; set; }
        [PasswordValidator] public string Password { get; set; }

    }
}
