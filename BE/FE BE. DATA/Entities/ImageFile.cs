using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.Entities
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public byte[] Content { get; set; }
        public int Size { get; set; }


        //[ForeignKey(nameof(User))]
        //public int? UserId { get; set; }//foreign key to user table, column, selects specific user
        //public User? User { get; set; }//reference User table(not column),for convenience, doesn't exist in DB, but shows how to load user

    }
}
