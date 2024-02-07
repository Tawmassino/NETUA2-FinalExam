using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface IUserMapper
    {
        User Map(UserCreateDTO dto);//create
        User Map(UserUpdateDTO dto);//update
        UserGetDTO Map(User user);//get
    }
}
