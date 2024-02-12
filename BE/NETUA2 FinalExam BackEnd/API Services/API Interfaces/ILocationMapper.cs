using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces
{
    public interface ILocationMapper
    {
        LivingLocation Map(LocationCreateDTO dto, int personId);//create
        LivingLocation Map(LocationUpdateDTO dto);//update
        LocationGetDTO Map(LivingLocation location);//get
    }
}
