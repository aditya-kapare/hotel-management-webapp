using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Services;


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
