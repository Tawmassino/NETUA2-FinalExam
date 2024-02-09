using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.Controllers;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services
{
    public class UserMapper : IUserMapper
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserMapper> _logger;
        public UserMapper(IUserService userService, ILogger<UserMapper> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // ===================== METHODS =====================
        /// <summary>
        /// CREATE: dto -> model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>User model</returns>
        public User Map(UserCreateDTO dto)
        {
            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                //Password = dto.Password,// no password 
                //must arrive as string to userService method CreatePasswordHash, where it's hashed/converted
            };
        }


        /// <summary>
        /// UPDATE: dto -> model
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>User Model</returns>
        public User Map(UserUpdateDTO dto)
        {
            _userService.CreatePasswordHash(dto.Password, out var passwordHash, out var passwordSalt);
            var entity = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Role = dto.Role,
            };
            return entity;
        }

        /// <summary>
        /// model -> dto
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User DTO</returns>
        public UserGetDTO Map(User user)
        {
            var entity = new UserGetDTO
            {
                //Id = user.Id,
                Username = user.Username,
                //Email = user.Email,
                //Password = user.Password, <- password is sensitive information and is not sent via DTOs
                //Role = user.Role,
            };
            return entity;
        }
    }
}
