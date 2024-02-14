using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.Entities
{
    public class LivingLocation
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public int? HouseNumber { get; set; }
        public string? Country { get; set; }



        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public Person Person { get; set; }



    }
}
