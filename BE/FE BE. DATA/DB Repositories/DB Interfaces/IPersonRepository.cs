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

        int AddNewPerson(Person person);

        Person GetPersonByUserId(int userId);
        Person GetPersonByPersonId(int personId);

        void UpdatePerson(Person person);

        void DeletePersonById(int userId);
    }
}
