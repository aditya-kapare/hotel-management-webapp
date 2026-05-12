using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing cab driver records.
    /// </summary>
    public class CabDriverDAL : ICabDriverDAL
    {
        private readonly HotelDbContext _context;


        public CabDriverDAL(HotelDbContext context)
        {
            // Assign database context
            _context = context;
        }

        //URL [HttpGet] - /api/cabdrivers/
        public async Task<IEnumerable<CabDriver>> GetAllDriversAsync()
        {
            // Fetch all drivers from database
            return await _context.CabDrivers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a cab driver by ID.
        /// </summary>

        //URL [HttpGet] - /api/cabdrivers/{driverId:int}
        public async Task<CabDriver?> GetDriverByIdAsync(int driverId)
        {
            // Find driver matching the given ID
            return await _context.CabDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);
        }

        //URL [HttpPost] - /api/cabdrivers
        public async Task<bool> AddDriverAsync(CabDriver driver)
        {
            // Add driver to context and persist
            await _context.CabDrivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return true;
        }

        //URL [HttpPut] - /api/cabdrivers
        public async Task<bool> UpdateDriverAsync(CabDriver driver)
        {
            // Mark driver entity as updated
            _context.CabDrivers.Update(driver);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrent update failure
                return false;
            }
        }

        //URL [HttpDelete] - /api/cabdrivers/{driverId:int}
        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            // Delete driver directly from database
            var affected = await _context.CabDrivers
                            .Where(d => d.DriverId == driverId)
                            .ExecuteDeleteAsync();

            return affected > 0;
        }
    }
}