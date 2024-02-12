using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services
{
    public class LocationMapper : ILocationMapper
    {
        public LivingLocation Map(LocationCreateDTO dto, int personId)
        {
            var location = new LivingLocation
            {
                City = dto.City,
                StreetName = dto.StreetName,
                HouseNumber = dto.HouseNumber,
                Country = dto.Country,
                PersonId = personId,
            };

            return location;
        }

        public LivingLocation Map(LocationUpdateDTO dto)
        {
            var location = new LivingLocation
            {
                City = dto.City,
                StreetName = dto.StreetName,
                HouseNumber = dto.HouseNumber,
                Country = dto.Country,
                PersonId = dto.PersonId,
            };

            return location;
        }

        public LocationGetDTO Map(LivingLocation location)
        {
            var entity = new LocationGetDTO
            {
                City = location.City,
                StreetName = location.StreetName,
                HouseNumber = location.HouseNumber,
                Country = location.Country,
                PersonId = location.PersonId,
            };
            return entity;
        }
    }
}
