using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string? Role { get; set; }


        [ForeignKey(nameof(Person))]
        public int? PersonId { get; set; }//atskiras stulpelis, pasako KURI person
        public Person? Person { get; set; }//cia pasakome kad yra Person tipo lentele, cia ne stuleplis, CIA DEL MUSU PATOGUMO, neegzistuoja DB, nuorodoa KAIP uzkrauti useri
    }
}
