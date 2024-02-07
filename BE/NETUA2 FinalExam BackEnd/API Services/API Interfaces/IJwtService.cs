namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(string username, string role, int userId);
    }
}
