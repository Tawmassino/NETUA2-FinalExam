using FE_BE._DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE_BE._DATA.DB_Repositories.DB_Interfaces
{
    public interface ILivingLocationRepository
    {
        int CreateNewLivingLocation(LivingLocation location);
        LivingLocation GetLocationByPersonId(int personId);
        LivingLocation GetLivingLocationByLocationId(int locationId);
        void UpdateLivingLocation(LivingLocation location);
        void DeleteLivingLocationByPersonId(int personId);
        void DeleteLivingLocationByLocationId(int locationId);
    }
}
