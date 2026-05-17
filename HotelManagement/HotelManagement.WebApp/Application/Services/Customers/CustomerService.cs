using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Services.Customers
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CustomerService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        // -------------------------
        // READ
        // -------------------------

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient.GetAsync("api/customers");

            if (!response.IsSuccessStatusCode)
                return new List<CustomerDto>();

            var customers = await response.Content
                .ReadFromJsonAsync<List<CustomerDto>>();

            return customers ?? new List<CustomerDto>();
        }

        public async Task<CustomerDto?> GetByIdentityIdAsync(string identityId)
        {
            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId))
                return null;

            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient
                .GetAsync($"api/customers/{identityId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<CustomerDto>();
        }

        public async Task<IReadOnlyList<CustomerDto>> GetByIdentityIdTypeAsync(
            IdentityIdType identityIdType)
        {
            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient
                .GetAsync($"api/customers/type/{(int)identityIdType}");

            if (!response.IsSuccessStatusCode)
                return new List<CustomerDto>();

            var customers = await response.Content
                .ReadFromJsonAsync<List<CustomerDto>>();

            return customers ?? new List<CustomerDto>();
        }

        // -------------------------
        // CREATE
        // -------------------------

        public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);

            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient 
                .PostAsJsonAsync("api/customers", normalized);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Customer with IdentityId '{normalized.IdentityId}' already exists or insert failed.");
            }

            return (await response.Content
                .ReadFromJsonAsync<CustomerDto>())!;
        }

        // -------------------------
        // UPDATE
        // -------------------------

        public async Task<CustomerDto> UpdateAsync(
    string identityId,
    UpdateCustomerRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId))
                throw new ArgumentException("IdentityId is required.", nameof(identityId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient
                .PutAsJsonAsync($"api/customers/{identityId}", normalized);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException(
                    $"Customer with IdentityId '{identityId}' was not found.");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Customer update failed.");


            return await GetByIdentityIdAsync(identityId)
                   ?? throw new InvalidOperationException("Updated customer not found.");
        }


        // -------------------------
        // DELETE
        // -------------------------

        public async Task<bool> DeleteAsync(string identityId)
        {
            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId))
                return false;

            var httpClient = _clientFactory.CreateClient("client");
            var response = await httpClient
                .DeleteAsync($"api/customers/{identityId}");

            return response.IsSuccessStatusCode;
        }

        // -------------------------
        // Normalization / Validation
        // -------------------------

        private static string NormalizeId(string id)
            => (id ?? string.Empty).Trim();

        private static CreateCustomerRequest NormalizeCreate(CreateCustomerRequest r)
            => new()
            {
                IdentityId = (r.IdentityId ?? string.Empty).Trim(),
                IdentityIdType = r.IdentityIdType,
                MobileNo = (r.MobileNo ?? string.Empty).Trim(),
                Name = (r.Name ?? string.Empty).Trim(),
                Gender = r.Gender,
                Address = (r.Address ?? string.Empty).Trim(),
                Country = (r.Country ?? string.Empty).Trim()
            };

        private static UpdateCustomerRequest NormalizeUpdate(UpdateCustomerRequest r)
            => new()
            {
                MobileNo = (r.MobileNo ?? string.Empty).Trim(),
                Name = (r.Name ?? string.Empty).Trim(),
                Gender = r.Gender,
                Address = (r.Address ?? string.Empty).Trim(),
                Country = (r.Country ?? string.Empty).Trim()
            };

        private static void ValidateCreate(CreateCustomerRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.IdentityId))
                throw new ArgumentException("IdentityId is required.", nameof(r.IdentityId));

            if (string.IsNullOrWhiteSpace(r.MobileNo))
                throw new ArgumentException("MobileNo is required.", nameof(r.MobileNo));

            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.", nameof(r.Name));
        }

        private static void ValidateUpdate(UpdateCustomerRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.MobileNo))
                throw new ArgumentException("MobileNo is required.", nameof(r.MobileNo));

            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.", nameof(r.Name));
        }
    }
}

