using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IRoomDAL
    {
        IEnumerable<Room> GetAllRooms();
        IEnumerable<Room> GetRoomsByType(int roomType);
        Room GetRoomByRoomNo(int roomNo);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int roomNo);
    }
}

