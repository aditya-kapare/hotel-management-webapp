using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Services;

//namespace HotelManagement.WebApp.Application.Services
//{
//    public sealed class RoomService : IRoomService
//    {
//        private readonly IRoomDAL _roomDal;

//        public RoomService(IRoomDAL roomDal)
//        {
//            _roomDal = roomDal;
//        }

//        public async Task<IReadOnlyList<RoomDto>> GetAllAsync()
//        {
//            var rooms = await _roomDal.GetAllRoomsAsync();
//            return rooms.Select(RoomMapping.ToDto).ToList();
//        }

//        public async Task<IReadOnlyList<RoomDto>> GetByTypeAsync(RoomType roomType)
//        {
//            var rooms = await _roomDal.GetRoomsByTypeAsync((int)roomType);
//            return rooms.Select(RoomMapping.ToDto).ToList();
//        }

//        public async Task<RoomDto?> GetByRoomNoAsync(int roomNo)
//        {
//            if (roomNo <= 0) return null;

//            var room = await _roomDal.GetRoomByRoomNoAsync(roomNo);
//            return room is null ? null : RoomMapping.ToDto(room);
//        }

//        public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
//        {
//            if (request is null) throw new ArgumentNullException(nameof(request));

//            ValidateCreate(request);

//            var entity = RoomMapping.ToEntity(request);

//            try
//            {
//                await _roomDal.AddRoomAsync(entity);
//            }
//            catch (DbUpdateException ex)
//            {
//                throw new InvalidOperationException(
//                    $"Room '{request.RoomNo}' already exists or insert failed.", ex);
//            }

//            return RoomMapping.ToDto(entity);
//        }

//        public async Task<RoomDto> UpdateAsync(int roomNo, UpdateRoomRequest request)
//        {
//            if (request is null) throw new ArgumentNullException(nameof(request));
//            if (roomNo <= 0) throw new ArgumentException("RoomNo must be positive.", nameof(roomNo));

//            ValidateUpdate(request);

//            var entity = RoomMapping.ToEntity(roomNo, request);

//            var updated = await _roomDal.UpdateRoomAsync(entity);
//            if (!updated)
//                throw new KeyNotFoundException($"Room '{roomNo}' was not found.");

//            return RoomMapping.ToDto(entity);
//        }

//        public async Task<bool> DeleteAsync(int roomNo)
//        {
//            if (roomNo <= 0) return false;

//            return await _roomDal.DeleteRoomAsync(roomNo);
//        }

//        private static void ValidateCreate(CreateRoomRequest r)
//        {
//            if (r.RoomNo <= 0)
//                throw new ArgumentException("RoomNo must be positive.", nameof(r.RoomNo));
//            if (r.Price < 0)
//                throw new ArgumentException("Price cannot be negative.", nameof(r.Price));
//        }

//        private static void ValidateUpdate(UpdateRoomRequest r)
//        {
//            if (r.Price < 0)
//                throw new ArgumentException("Price cannot be negative.", nameof(r.Price));
//        }
//    }
//}

using System.Net;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class RoomService : IRoomService
    {
        private const string BaseRoute = "api/rooms";
        private readonly IHttpClientFactory _httpClientFactory;

        public RoomService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
            => _httpClientFactory.CreateClient("client");

        // -------------------------
        // READ
        // -------------------------
        public async Task<IReadOnlyList<RoomDTO>> GetAllAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseRoute);

            if (!response.IsSuccessStatusCode)
                return new List<RoomDTO>();

            return await response.Content.ReadFromJsonAsync<List<RoomDTO>>()
                   ?? new List<RoomDTO>();
        }

        public async Task<RoomDTO?> GetByRoomNoAsync(int roomNo)
        {
            if (roomNo <= 0) return null;

            var client = CreateClient();
            var response = await client.GetAsync($"{BaseRoute}/{roomNo}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<RoomDTO>();
        }

        public async Task<IReadOnlyList<RoomDTO>> GetByRoomTypeAsync(int roomType)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"{BaseRoute}/by-type/{roomType}");

            if (!response.IsSuccessStatusCode)
                return new List<RoomDTO>();

            return await response.Content.ReadFromJsonAsync<List<RoomDTO>>()
                   ?? new List<RoomDTO>();
        }

        // -------------------------
        // CREATE
        // -------------------------
        public async Task<RoomDTO> CreateAsync(RoomForCreationDTO request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidateCreate(request);

            var client = CreateClient();
            var response = await client.PostAsJsonAsync(BaseRoute, request);

            // ✅ NEW: handle duplicate room explicitly
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var apiMessage = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(apiMessage);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<RoomDTO>())!;
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public async Task<RoomDTO> UpdateAsync(int roomNo, RoomForUpdateDTO request)
        {
            if (roomNo <= 0)
                throw new ArgumentException("Room number must be positive.", nameof(roomNo));
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidateUpdate(request);

            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"{BaseRoute}/{roomNo}", request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException($"Room '{roomNo}' not found.");

            // ✅ NEW: handle conflict
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var apiMessage = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(apiMessage);
            }

            response.EnsureSuccessStatusCode();

            return await GetByRoomNoAsync(roomNo)
                ?? throw new InvalidOperationException("Updated room not found.");
        }


        // -------------------------
        // DELETE
        // -------------------------
        public async Task<bool> DeleteAsync(int roomNo)
        {
            if (roomNo <= 0) return false;

            var client = CreateClient();
            var response = await client.DeleteAsync($"{BaseRoute}/{roomNo}");

            return response.IsSuccessStatusCode;
        }

        // -------------------------
        // Validation helpers
        // -------------------------
        private static void ValidateCreate(RoomForCreationDTO r)
        {
            if (r.RoomNo <= 0)
                throw new ArgumentException("RoomNo must be positive.");
            if (r.Price < 0)
                throw new ArgumentException("Price cannot be negative.");
        }

        private static void ValidateUpdate(RoomForUpdateDTO r)
        {
            if (r.Price < 0)
                throw new ArgumentException("Price cannot be negative.");
        }
    }
}
