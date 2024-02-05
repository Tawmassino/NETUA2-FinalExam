namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class LoginResponse : UserResponse
    {
        public string Token { get; set; }

        public LoginResponse(bool isSuccess, string message, string token) : base(isSuccess, message)
        {
            Token = token;
        }
    }
}
