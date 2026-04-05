using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<IReadOnlyList<RoomDto>> GetAllAsync();
        Task<IReadOnlyList<RoomDto>> GetByTypeAsync(RoomType roomType);
        Task<RoomDto?> GetByRoomNoAsync(int roomNo);

        Task<RoomDto> CreateAsync(CreateRoomRequest request);
        Task<RoomDto> UpdateAsync(int roomNo, UpdateRoomRequest request);

        Task<bool> DeleteAsync(int roomNo);
    }
}