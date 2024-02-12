using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories
{
    public class LivingLocationRepository : ILivingLocationRepository
    {

        private readonly FinalExamDbContext _dbContext;
        private readonly ILogger<LivingLocationRepository> _logger;

        public LivingLocationRepository(FinalExamDbContext dbContext, ILogger<LivingLocationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // ================================ METHODS ================================

        public int CreateNewLivingLocation(LivingLocation location)
        {
            _logger.LogInformation($"Creating a location with ID:{location.Id} for person with id {location.PersonId}.");

            _dbContext.Locations.Add(location);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Location with ID: {location.Id} has been successfully created for person with id {location.PersonId}.");
            return location.Id;
        }

        public void DeleteLivingLocationByLocationId(int locationId)
        {
            var locationToDelete = _dbContext.Locations.Find(locationId);
            if (locationToDelete != null)
            {
                _dbContext.Locations.Remove(locationToDelete);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Location with ID: {locationId} has been successfully deleted ");
            }
        }

        public void DeleteLivingLocationByPersonId(int personId)
        {
            var locationToDelete = GetLocationByPersonId(personId);
            if (locationToDelete != null)
            {
                _dbContext.Locations.Remove(locationToDelete);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Location with ID: {locationToDelete.Id} has been successfully deleted ");
            }
        }


        public LivingLocation GetLocationByPersonId(int personId)
        {
            return _dbContext.Locations.Include(location => location.Person).FirstOrDefault(location => location.PersonId == personId);//ok           
        }



        public LivingLocation GetLivingLocationByLocationId(int locationId)
        {
            return _dbContext.Locations.Include(include => include.Person).FirstOrDefault(location => location.Id == locationId);
        }



        public void UpdateLivingLocation(LivingLocation location)
        {
            _dbContext.Locations.Update(location);
            _dbContext.SaveChanges();
        }




    }
}
