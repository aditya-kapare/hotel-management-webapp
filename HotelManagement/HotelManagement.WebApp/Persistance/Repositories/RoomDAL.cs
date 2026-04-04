using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.DAL
{
    public class RoomDAL : IRoomDAL
    {
        private readonly HotelDbContext _context;

        public RoomDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _context.Rooms.ToList();
        }

        public IEnumerable<Room> GetRoomsByType(int roomType)
        {
            return _context.Rooms.Where(r => (int)r.RoomType == roomType).ToList();
        }

        public Room GetRoomByRoomNo(int roomNo)
            => _context.Rooms.FirstOrDefault(r => r.RoomNo == roomNo);

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }

        public void DeleteRoom(int roomNo)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomNo == roomNo);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}