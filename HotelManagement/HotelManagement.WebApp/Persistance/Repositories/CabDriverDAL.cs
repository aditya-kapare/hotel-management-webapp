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

        public async Task AddDriverAsync(CabDriver driver)
        {
            await _context.CabDrivers.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(CabDriver driver)
        {
            _context.CabDrivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(int driverId)
        {
            var driver = await _context.CabDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);

            if (driver != null)
            {
                _context.CabDrivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }
    }
}