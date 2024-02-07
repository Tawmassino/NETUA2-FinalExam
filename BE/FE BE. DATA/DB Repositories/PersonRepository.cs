using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories
{
    public class PersonRepository : IPersonRepository
    {

        private readonly FinalExamDbContext _dbContext;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(FinalExamDbContext dbContext, ILogger<PersonRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // ================================ METHODS ================================

        public Person CreateNewPerson(int userId)
        {
            _logger.LogInformation($"Creating a person with ID: {userId}");
            Person newPerson = (new Person
            {
                Name = "",
                Surname = "",
                SocialSecurityNumber = "",
                PhoneNumber = "",
                Email = "",
                UserId = userId,
                UserLocationId = null,
                ProfilePictureId = null,
            });

            _dbContext.Persons.Add(newPerson);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Person with ID: {userId} has been successfully created.");
            return newPerson;
        }

        public void DeletePersonById(int userId)
        {
            var personToDelete = _dbContext.Persons.Find(userId);
            if (personToDelete != null)
            {
                _dbContext.Persons.Remove(personToDelete);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Person (ID: {userId}) {personToDelete.Name} {personToDelete.Surname} has been successfully deleted");
            }
        }

        public Person GetPersonByUserId(int userId)
        {
            return _dbContext.Persons.Include(person => person.UserId).FirstOrDefault(person => person.UserId == userId);
        }

        public Person GetPersonByPersonId(int personId)
        {
            return _dbContext.Persons.Include(include => include.User).FirstOrDefault(person => person.Id == personId);
        }


        public void UpdatePerson(Person person)//gal nereik userid ateina is controller mapper
        {
            _dbContext.Persons.Update(person);
            _dbContext.SaveChanges();
        }
    }
}
