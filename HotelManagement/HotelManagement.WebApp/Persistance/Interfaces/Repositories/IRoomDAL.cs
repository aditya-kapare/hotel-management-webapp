using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IRoomDAL
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomType);
        Task<Room?> GetRoomByRoomNoAsync(int roomNo);
        Task<bool> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Room room);
        Task<bool> DeleteRoomAsync(int roomNo);
    }
}
