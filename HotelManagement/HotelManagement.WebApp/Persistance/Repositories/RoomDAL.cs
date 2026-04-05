using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    public class RoomDAL : IRoomDAL
    {
        private readonly HotelDbContext _context;

        public RoomDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomType)
        {
            return await _context.Rooms
                .Where(r => (int)r.RoomType == roomType)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByRoomNoAsync(int roomNo)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNo == roomNo);
        }

        public async Task AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(int roomNo)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNo == roomNo);

            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}