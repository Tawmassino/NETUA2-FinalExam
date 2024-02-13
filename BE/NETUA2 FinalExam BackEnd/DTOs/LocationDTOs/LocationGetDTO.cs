using FE_BE._DATA.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs
{
    public class LocationGetDTO
    {
        public string City { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string Country { get; set; }


        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
