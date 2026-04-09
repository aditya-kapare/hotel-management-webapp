using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

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

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {

            var affected = await _context.CabDrivers
                            .Where(d => d.DriverId == driverId)
                            .ExecuteDeleteAsync();

            return affected > 0;

        }
    }
}