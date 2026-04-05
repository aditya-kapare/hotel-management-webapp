using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Rooms;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class RoomService : IRoomService
    {
        private readonly IRoomDAL _roomDal;

        public RoomService(IRoomDAL roomDal)
        {
            _roomDal = roomDal;
        }

        public async Task<IReadOnlyList<RoomDto>> GetAllAsync()
        {
            var rooms = await _roomDal.GetAllRoomsAsync();
            return rooms.Select(RoomMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<RoomDto>> GetByTypeAsync(RoomType roomType)
        {
            // DAL expects int roomType
            var rooms = await _roomDal.GetRoomsByTypeAsync((int)roomType);
            return rooms.Select(RoomMapping.ToDto).ToList();
        }

        public async Task<RoomDto?> GetByRoomNoAsync(int roomNo)
        {
            if (roomNo <= 0) return null;

            var room = await _roomDal.GetRoomByRoomNoAsync(roomNo);
            return room is null ? null : RoomMapping.ToDto(room);
        }

        public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            ValidateCreate(request);

            // prevent duplicate RoomNo
            var existing = await _roomDal.GetRoomByRoomNoAsync(request.RoomNo);
            if (existing is not null)
                throw new InvalidOperationException($"Room '{request.RoomNo}' already exists.");

            var entity = RoomMapping.ToEntity(request);

            await _roomDal.AddRoomAsync(entity);

            return RoomMapping.ToDto(entity);
        }

        public async Task<RoomDto> UpdateAsync(int roomNo, UpdateRoomRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (roomNo <= 0) throw new ArgumentException("RoomNo must be positive.", nameof(roomNo));

            ValidateUpdate(request);

            var existing = await _roomDal.GetRoomByRoomNoAsync(roomNo);
            if (existing is null)
                throw new KeyNotFoundException($"Room '{roomNo}' was not found.");

            RoomMapping.Apply(request, existing);

            await _roomDal.UpdateRoomAsync(existing);

            return RoomMapping.ToDto(existing);
        }

        public async Task<bool> DeleteAsync(int roomNo)
        {
            if (roomNo <= 0) return false;

            var existing = await _roomDal.GetRoomByRoomNoAsync(roomNo);
            if (existing is null) return false;

            await _roomDal.DeleteRoomAsync(roomNo);
            return true;
        }

        // Minimal validation helpers
        private static void ValidateCreate(CreateRoomRequest r)
        {
            if (r.RoomNo <= 0)
                throw new ArgumentException("RoomNo must be positive.", nameof(r.RoomNo));

            if (r.Price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(r.Price));
        }

        private static void ValidateUpdate(UpdateRoomRequest r)
        {
            if (r.Price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(r.Price));
        }
    }
}