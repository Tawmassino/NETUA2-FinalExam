using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Interfaces
{
    public interface IUserDBRepository
    {
        public User GetUserByUsername(string username);
        public User GetUserById(int userId);
        public int AddNewUser(User user);
        void DeleteUser(int userId);
    }
}
