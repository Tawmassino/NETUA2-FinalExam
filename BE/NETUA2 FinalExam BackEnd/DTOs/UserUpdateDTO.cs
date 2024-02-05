﻿using NETUA2_FinalExam_BackEnd.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class UserUpdateDTO
    {
        //ID is not required in updateDTO, only in get method
        [Required][StringLength(30, MinimumLength = 3)] public string Username { get; set; }
        [EmailAddress] public string? Email { get; set; }
        [PasswordValidator] public string Password { get; set; }
        public string Role { get; set; }
    }
}
