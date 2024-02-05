namespace NETUA2_FinalExam_BackEnd.DTOs
{
    public class UserResponse
    {
        public int UserId { get; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

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
