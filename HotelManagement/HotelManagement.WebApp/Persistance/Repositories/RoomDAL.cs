using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing room records.
    /// </summary>
    public class RoomDAL : IRoomDAL
    {
        private readonly HotelDbContext _context;

    
        public RoomDAL(HotelDbContext context)
        {
           
            _context = context;
        }

        /// <summary>
        /// Retrieves all rooms.
        /// </summary>
        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            // Fetch all rooms from database
            return await _context.Rooms.ToListAsync();
        }

      
        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomType)
        {
            // Filter rooms by room type
            return await _context.Rooms
                .Where(r => (int)r.RoomType == roomType)
                .ToListAsync();
        }

        
        public async Task<Room?> GetRoomByRoomNoAsync(int roomNo)
        {
            // Find room matching the room number
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNo == roomNo);
        }

      
        public async Task<bool> AddRoomAsync(Room room)
        {
         
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
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

        
        public async Task<bool> DeleteRoomAsync(int roomNo)
        {
       
            var room = await _context.Rooms
                .Where(r => r.RoomNo == roomNo)
                .ExecuteDeleteAsync();

            return room > 0;
        }
    }
}
