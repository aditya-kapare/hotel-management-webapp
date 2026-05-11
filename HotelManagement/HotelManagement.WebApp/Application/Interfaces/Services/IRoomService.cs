using HotelManagement.WebApp.Application.Dtos.Room;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<IReadOnlyList<RoomDTO>> GetAllAsync();
        Task<RoomDTO?> GetByRoomNoAsync(int roomNo);
        Task<IReadOnlyList<RoomDTO>> GetByRoomTypeAsync(int roomType);
        Task<RoomDTO> CreateAsync(RoomForCreationDTO request);
        Task<RoomDTO> UpdateAsync(int roomNo, RoomForUpdateDTO request);
        Task<bool> DeleteAsync(int roomNo);
    }
}