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

        /// <summary>
        /// Adds a new person to the database and logs relevant information.
        /// </summary>
        /// <param name="person">An instance of the Person class representing the person to be added.</param>
        /// <returns>An integer representing the ID of the newly created person.</returns>
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



        /// <summary>
        /// Deletes a person from the database based on the provided person ID and logs relevant information.
        /// </summary>
        /// <param name="personId">An integer representing the ID of the person to be deleted.</param>
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


        /// <summary>
        /// Retrieves a person from the database based on the provided user ID, including user information if available.
        /// </summary>
        /// <param name="userId">An integer representing the user ID associated with the person.</param>
        /// <returns>A Person object representing the retrieved person.</returns>
        public Person GetPersonByUserId(int userId)
        {
            //return _dbContext.Persons.Include(person => person.User).FirstOrDefault(person => person.UserId == userId);//ok
            return _dbContext.Persons.Include(person => person.User).FirstOrDefault(person => person.UserId == userId && person.User != null);
            //ne include userId
        }


        /// <summary>
        /// Retrieves the living location of a person from the database based on the provided person ID.
        /// </summary>
        /// <param name="personId">An integer representing the ID of the person whose location is to be retrieved.</param>
        /// <returns>A LivingLocation object representing the retrieved living location.</returns>
        public LivingLocation GetLocationByPersonId(int personId)
        {
            return _dbContext.Locations.Include(location => location.Person).FirstOrDefault(location => location.PersonId == personId);//ok           
        }


        /// <summary>
        /// Retrieves a person from the database based on the provided person ID, including user information if available.
        /// </summary>
        /// <param name="personId">An integer representing the ID of the person to be retrieved.</param>
        /// <returns>A Person object representing the retrieved person.</returns>
        public Person GetPersonByPersonId(int personId)
        {
            return _dbContext.Persons.Include(include => include.User).FirstOrDefault(person => person.Id == personId);//ok
        }


        /// <summary>
        /// Updates the information of an existing person in the database based on the provided person object.
        /// </summary>
        /// <param name="person">An instance of the Person class containing the updated information.</param>        
        public void UpdatePerson(Person person)
        {
            _dbContext.Persons.Update(person);
            _dbContext.SaveChanges();
        }


    }
}
