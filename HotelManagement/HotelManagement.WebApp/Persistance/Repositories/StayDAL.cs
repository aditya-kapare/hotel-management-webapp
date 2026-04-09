using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    public class StayDAL : IStayDAL
    {
        private readonly HotelDbContext _context;

        public StayDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stay>> GetAllStaysAsync()
        {
            return await _context.Stays.ToListAsync();
        }

        public async Task<Stay?> GetStayByIdAsync(int stayId)
        {
            return await _context.Stays
                .FirstOrDefaultAsync(s => s.StayId == stayId);
        }

        public async Task<IEnumerable<Stay>> GetStaysByRoomNoAsync(int roomNo)
        {
            return await _context.Stays
                .Where(s => s.RoomNo == roomNo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCustomerIdentityIdAsync(string customerIdentityId)
        {
            return await _context.Stays
                .Where(s => s.CustomerIdentityId == customerIdentityId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCheckInDateAsync(DateTime date)
        {
            var target = date.Date;
            return await _context.Stays
                .Where(s => s.CheckInAt.Date == target)
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