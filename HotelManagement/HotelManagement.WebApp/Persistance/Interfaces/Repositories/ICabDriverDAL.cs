using HotelManagement.WebApp.Domain.Models;
using System.Collections.Generic;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICabDriverDAL
    {
        IEnumerable<CabDriver> GetAllDrivers();
        CabDriver GetDriverById(int driverId);
        void AddDriver(CabDriver driver);
        void UpdateDriver(CabDriver driver);
        void DeleteDriver(int driverId);
    }
}