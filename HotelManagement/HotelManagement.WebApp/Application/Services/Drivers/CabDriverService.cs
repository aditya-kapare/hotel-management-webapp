using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using System.Net;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class CabDriverService : ICabDriverService
    {
        private const string BaseRoute = "api/cabdrivers";
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly HttpClient _httpClient;

        public CabDriverService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // -------------------------
        // READ
        // -------------------------
        public async Task<IReadOnlyList<CabDriverDTO>> GetAllAsync()
        {
            var _httpClient = _httpClientFactory.CreateClient("client");
            var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<CabDriverDTO>>(BaseRoute);
            if (response is null)
                return new List<CabDriverDTO>();

            return response;
        }

        public async Task<CabDriverDTO?> GetByIdAsync(int driverId)
        {
            var _httpClient = _httpClientFactory.CreateClient("client");
            if (driverId <= 0) return null;

            var response = await _httpClient.GetAsync($"{BaseRoute}/{driverId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CabDriverDTO>();
        }

        public async Task<CabDriverDTO?> GetByGovtIdAsync(string govtId)
        {
            var _httpClient = _httpClientFactory.CreateClient("client");
            govtId = NormalizeId(govtId);
            if (string.IsNullOrWhiteSpace(govtId)) return null;

            var response = await _httpClient.GetAsync($"{BaseRoute}/by-govt-id/{govtId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CabDriverDTO>();
        }

        // -------------------------
        // CREATE
        // -------------------------
        public async Task<CabDriverDTO> CreateAsync(CabDriverForCreationDTO request)
        {
            var _httpClient = _httpClientFactory.CreateClient("client");

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidateCreate(request);

            var response = await _httpClient.PostAsJsonAsync(BaseRoute, request);

            // ✅ HANDLE DUPLICATE (409)
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var apiMessage = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(apiMessage);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<CabDriverDTO>())!;
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public async Task<CabDriverDTO> UpdateByIdAsync(
            int driverId,
            CabDriverForUpdateDTO request)
        {
            if (driverId <= 0)
                throw new ArgumentException("DriverId must be positive.", nameof(driverId));
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidateUpdate(request);
            var _httpClient = _httpClientFactory.CreateClient("client");
            var response = await _httpClient
                .PutAsJsonAsync($"{BaseRoute}/{driverId}", request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException($"Driver '{driverId}' not found.");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Cab driver update failed.");

            return await GetByIdAsync(driverId)
                   ?? throw new InvalidOperationException("Updated driver not found.");
        }

        public async Task<CabDriverDTO> UpdateByGovtIdAsync(
            string govtId,
            CabDriverForUpdateDTO request)
        {
            govtId = NormalizeId(govtId);
            if (string.IsNullOrWhiteSpace(govtId))
                throw new ArgumentException("GovernmentId is required.", nameof(govtId));
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidateUpdate(request);
            var _httpClient = _httpClientFactory.CreateClient("client");
            var response = await _httpClient
                .PutAsJsonAsync($"{BaseRoute}/by-govt-id/{govtId}", request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException($"Driver '{govtId}' not found.");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Cab driver update failed.");

            return await GetByGovtIdAsync(govtId)
                   ?? throw new InvalidOperationException("Updated driver not found.");
        }

        // -------------------------
        // DELETE
        // -------------------------
        public async Task<bool> DeleteByIdAsync(int driverId)
        {
            if (driverId <= 0) return false;
            var _httpClient = _httpClientFactory.CreateClient("client");
            var response = await _httpClient.DeleteAsync($"{BaseRoute}/{driverId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteByGovtIdAsync(string govtId)
        {
            govtId = NormalizeId(govtId);
            if (string.IsNullOrWhiteSpace(govtId)) return false;
            var _httpClient = _httpClientFactory.CreateClient("client");
            var response = await _httpClient
                .DeleteAsync($"{BaseRoute}/by-govt-id/{govtId}");

            return response.IsSuccessStatusCode;
        }

        // -------------------------
        // Validation helpers
        // -------------------------
        private static string NormalizeId(string id)
            => (id ?? string.Empty).Trim();

        private static void ValidateCreate(CabDriverForCreationDTO r)
        {
            if (string.IsNullOrWhiteSpace(r.GovernmentId))
                throw new ArgumentException("GovernmentId is required.");
            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.");
            if (r.Age <= 0)
                throw new ArgumentException("Age must be positive.");
            if (string.IsNullOrWhiteSpace(r.CarVendor))
                throw new ArgumentException("CarVendor is required.");
            if (string.IsNullOrWhiteSpace(r.CarType))
                throw new ArgumentException("CarType is required.");
        }

        private static void ValidateUpdate(CabDriverForUpdateDTO r)
        {
            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.");
            if (r.Age <= 0)
                throw new ArgumentException("Age must be positive.");
            if (string.IsNullOrWhiteSpace(r.CarVendor))
                throw new ArgumentException("CarVendor is required.");
            if (string.IsNullOrWhiteSpace(r.CarType))
                throw new ArgumentException("CarType is required.");
        }
    }
}