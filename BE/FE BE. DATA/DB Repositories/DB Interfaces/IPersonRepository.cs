using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories.DB_Interfaces
{
    public interface IPersonRepository
    {

        Person CreateNewPerson(int userId);

        Person GetPersonByUserId(int userId);

        void UpdatePerson(Person person);

        void DeletePersonById(int userId);
        Person GetPersonByPersonId(int personId);
    }
}
