using System.ComponentModel.DataAnnotations;

namespace NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs
{
    public class LocationCreateDTO
    {
        [Required]public string City { get; set; }
        [Required] public string StreetName { get; set; }
        [Required] public int HouseNumber { get; set; }
        [Required] public string Country { get; set; }


        public int? PersonId { get; set; }//the user does not fill in this info. It arrives from elsewhere.
    }
}
