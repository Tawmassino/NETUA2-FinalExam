using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface IUserService
    {
        UserResponse Register(string username, string email, string password);
        (bool, User) Login(string username, string password, out string role);
        int GetCurrentUserId();

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        Person CreateNewPerson(int userId);
    }
}
