using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Rooms;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

            var entity = RoomMapping.ToEntity(request);

            try
            {
                await _roomDal.AddRoomAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Room '{request.RoomNo}' already exists or insert failed.", ex);
            }

            return RoomMapping.ToDto(entity);
        }

        public async Task<RoomDto> UpdateAsync(int roomNo, UpdateRoomRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (roomNo <= 0) throw new ArgumentException("RoomNo must be positive.", nameof(roomNo));

            ValidateUpdate(request);

            var entity = RoomMapping.ToEntity(roomNo, request);

            var updated = await _roomDal.UpdateRoomAsync(entity);
            if (!updated)
                throw new KeyNotFoundException($"Room '{roomNo}' was not found.");

            return RoomMapping.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(int roomNo)
        {
            if (roomNo <= 0) return false;

            return await _roomDal.DeleteRoomAsync(roomNo);
        }

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