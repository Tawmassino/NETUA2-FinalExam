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

        public int AddNewPerson(Person person)
        {
            _logger.LogInformation($"Creating a person with ID: {person.Id}");

            //check if doesnt exist already
            var existingPerson = GetPersonByUserId(person.Id);
            if (existingPerson == null)
            {
                _dbContext.Persons.Add(person);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Person {person.Name} {person.Surname} with ID: {person.Id} has been successfully created.");
            }
            return person.Id;
        }

        public void DeletePersonById(int personId)
        {
            var personToDelete = _dbContext.Persons.Find(personId);
            if (personToDelete != null)
            {
                _dbContext.Persons.Remove(personToDelete);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Person (ID: {personId}) {personToDelete.Name} {personToDelete.Surname} has been successfully deleted");
            }
        }

        public Person GetPersonByUserId(int userId)
        {
            return _dbContext.Persons.Include(person => person.UserId).FirstOrDefault(person => person.UserId == userId);//ok
        }

        public Person GetPersonByPersonId(int personId)
        {
            return _dbContext.Persons.Include(include => include.User).FirstOrDefault(person => person.Id == personId);//ok
        }


        public void UpdatePerson(Person person)
        {
            _dbContext.Persons.Update(person);
            _dbContext.SaveChanges();
        }
    }
}
