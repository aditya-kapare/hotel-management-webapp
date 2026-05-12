using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing stay records.
    /// </summary>
    public class StayDAL : IStayDAL
    {
        private readonly HotelDbContext _context;

        public StayDAL(HotelDbContext context)
        {

            _context = context;
        }


        public async Task<IEnumerable<Stay>> GetAllStaysAsync()
        {
            // Load stays with related customer
            return await _context.Stays
                                 .Include(s => s.Customer).ToListAsync();
        }


        public async Task<Stay?> GetStayByIdAsync(int stayId)
        {
            // Find stay matching the given ID
            return await _context.Stays
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.StayId == stayId);
        }


        public async Task<IEnumerable<Stay>> GetStaysByRoomNoAsync(int roomNo)
        {
            // Filter stays by room number
            return await _context.Stays
                .Where(s => s.RoomNo == roomNo)
                .Include(s => s.Customer)
                .ToListAsync();
        }


        public async Task<IEnumerable<Stay>> GetStaysByCustomerIdentityIdAsync(string customerIdentityId)
        {
            // Filter stays by customer identity ID
            return await _context.Stays
                .Where(s => s.CustomerIdentityId == customerIdentityId)
                .Include(s => s.Customer)
                .ToListAsync();
        }


        public async Task<IEnumerable<Stay>> GetStaysByCheckInDateAsync(DateTime date)
        {
            // Normalize date for comparison
            var target = date.Date;
            return await _context.Stays
                .Where(s => s.CheckInAt.Date == target)
                .Include(s => s.Customer)
                .ToListAsync();
        }

        public async Task<bool> AddStayAsync(Stay stay)
        {

            await _context.Stays.AddAsync(stay);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStayAsync(Stay stay)
        {

            _context.Stays.Update(stay);
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


        public async Task<bool> DeleteStayAsync(int stayId)
        {

            var stay = await _context.Stays
                .Where(s => s.StayId == stayId)
                .ExecuteDeleteAsync();

            return stay > 0;
        }
    }
}