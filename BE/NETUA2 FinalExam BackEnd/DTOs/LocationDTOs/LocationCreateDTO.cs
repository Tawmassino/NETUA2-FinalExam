namespace NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs
{
    public class LocationCreateDTO
    {
        public string City { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string Country { get; set; }


        public int? PersonId { get; set; }//the user does not fill in this info. It arrives from elsewhere.
    }
}
