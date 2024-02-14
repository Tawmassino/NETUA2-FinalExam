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
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }


        [ForeignKey(nameof(User))]
        public int UserId { get; set; }//foreign key to user table, column , says which  user
        public User User { get; set; }// table with User type, not a column, convenience only, doesnt exist in database, reference how to load user
        //amzinas ciklas


        //Location depends on Person, not vice versa
        //[ForeignKey(nameof(LivingLocation))]
        //public int? UserLocationId { get; set; }//foreign key to location table, column , says which  user
        //public LivingLocation? LivingLocation { get; set; }//table with Location type, not a column, convenience only, doesnt exist in database, reference how to load user



        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePictureId { get; set; }
        public ImageFile? ProfilePicture { get; set; }

    }
}
