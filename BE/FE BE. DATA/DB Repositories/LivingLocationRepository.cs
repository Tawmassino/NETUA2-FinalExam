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

        public LivingLocation CreateNewLivingLocation(int personId)
        {
            _logger.LogInformation($"Creating a person with ID: {personId}");
            LivingLocation newLocation = (new LivingLocation
            {
                City = "",
                StreetName = "",
                HouseNumber = 0,
                Country = "",
            });

            _dbContext.Locations.Add(newLocation);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Person with ID: {personId} has been successfully created.");
            return newLocation;
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
            throw new NotImplementedException();
        }


        public LivingLocation GetLivingLocationByPersonId(int personId)
        {
            return _dbContext.Locations.Include(location => location.Person).FirstOrDefault(location => location.PersonId == personId);
        }

        public LivingLocation GetLivingLocationByLocationId(int locationId)
        {
            return _dbContext.Locations.Include(include => include.Person).FirstOrDefault(location => location.Id == locationId);
        }



        public void UpdateLivingLocation(LivingLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
