using NETUA2_FinalExam_BackEnd.DTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface IUserService
    {
        UserResponse Register(string username, string email, string password);
        UserResponse Login(string username, string password, out string role);
        int GetCurrentUserId();

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
