using FE_BE._DATA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA
{
    public class FinalExamDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<LivingLocation> Locations { get; set; }

        public DbSet<ImageFile> Images { get; set; }

        public FinalExamDbContext(DbContextOptions<FinalExamDbContext> options) : base(options)
        {
        }
    }
}
