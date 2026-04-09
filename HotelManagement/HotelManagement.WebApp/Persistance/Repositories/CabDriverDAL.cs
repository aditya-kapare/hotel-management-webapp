using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<CabDriver>> GetAllDriversAsync()
        {
            return await _context.CabDrivers.ToListAsync();
        }

        public async Task<CabDriver?> GetDriverByIdAsync(int driverId)
        {
            return await _context.CabDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);
        }

        public async Task<bool> AddDriverAsync(CabDriver driver)
        {
            await _context.CabDrivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDriverAsync(CabDriver driver)
        {
            _context.CabDrivers.Update(driver);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            var driver = await _context.CabDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);

            if (driver is null) return false;

            _context.CabDrivers.Remove(driver);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}