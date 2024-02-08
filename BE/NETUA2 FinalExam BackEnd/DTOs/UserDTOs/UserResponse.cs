namespace NETUA2_FinalExam_BackEnd.DTOs.UserDTOs
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Jwt { get; set; }

        public UserResponse(bool isSuccess, string message, string jwt, int userId)
        {
            IsSuccess = isSuccess;
            Message = message;
            Jwt = jwt;
            UserId = userId;
        }
        public UserResponse(bool isSuccess, string message, string jwt)
        {
            IsSuccess = isSuccess;
            Message = message;
            Jwt = jwt;
        }

        public UserResponse(bool isSuccess, string message, int userId)
        {
            IsSuccess = isSuccess;
            Message = message;
            UserId = userId;
        }
        public UserResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public UserResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
