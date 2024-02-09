using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface IPersonMapper
    {
        Person Map(PersonCreateDTO dto, int userId);//create
        Person Map(PersonUpdateDTO dto);//update
        PersonGetDTO Map(User user);//get
    }
}
