using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.DAL
{
    public class CabDriverDAL : ICabDriverDAL
    {
        private readonly HotelDbContext _context;

        public CabDriverDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CabDriver> GetAllDrivers()
        {
            return _context.CabDrivers.ToList();
        }

        public CabDriver GetDriverById(int driverId)
            => _context.CabDrivers.FirstOrDefault(d => d.DriverId == driverId);

        public void AddDriver(CabDriver driver)
        {
            _context.CabDrivers.Add(driver);
            _context.SaveChanges();
        }

        public void UpdateDriver(CabDriver driver)
        {
            _context.CabDrivers.Update(driver);
            _context.SaveChanges();
        }

        public void DeleteDriver(int driverId)
        {
            var driver = _context.CabDrivers.FirstOrDefault(d => d.DriverId == driverId);
            if (driver != null)
            {
                _context.CabDrivers.Remove(driver);
                _context.SaveChanges();
            }
        }
    }
}