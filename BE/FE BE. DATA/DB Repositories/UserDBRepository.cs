using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories
{
    public class UserDBRepository : IUserDBRepository
    {
        private readonly FinalExamDbContext _dbContext;
        private readonly ILogger<UserDBRepository> _logger;

        public UserDBRepository(FinalExamDbContext dbContext, ILogger<UserDBRepository> logger)
        {
            //_dbContext.Database.EnsureCreated();//jei yra sita eilute, is viso nereikia migraciju. DB kurse
            //kol nera duomenu (per swagger irasyt) tol DB nera
            _dbContext = dbContext;
            _logger = logger;
        }
        // ==================== methods ====================

        public User GetUserByUsername(string username)
        {
            return _dbContext.Users.SingleOrDefault(user => user.Username == username);
        }

        public User GetUserById(int userId)
        {
            return _dbContext.Users.SingleOrDefault(user => user.Id == userId);
        }


        public int AddNewUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }


        public void DeleteUser(int userId)
        {
            var userToDelete = _dbContext.Users.Find(userId);
            if (userToDelete != null)
            {
                _dbContext.Remove(userToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
