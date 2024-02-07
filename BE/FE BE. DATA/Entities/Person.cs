using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }

        //public string Location { get; set; }



        [ForeignKey(nameof(User))]
        //yra sarysis is user puses, cia nerikia
        public int UserId { get; set; }//foreign key to user table, atskiras stulpelis, pasako KURI  useri
        public User User { get; set; }//cia pasakome kad yra  User tipo lentele, cia ne stuleplis, CIA DEL MUSU PATOGUMO, neegzistuoja DB, nuorodoa KAIP uzkrauti useri
        //amzinas ciklas


        [ForeignKey(nameof(LivingLocation))]
        public int? UserLocationId { get; set; }//nurodo foreign key i user lentele, atskiras stulpelis, pasako KURI  useri
        public LivingLocation? LivingLocation { get; set; }//cia pasakome kad yra  User tipo lentele, cia ne stuleplis, CIA DEL MUSU PATOGUMO, neegzistuoja DB, nuorodoa KAIP uzkrauti useri


        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePictureId { get; set; }
        public ImageFile? ProfilePicture { get; set; }

    }
}
