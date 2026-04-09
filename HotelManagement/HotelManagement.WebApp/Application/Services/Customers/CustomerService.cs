using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Customers;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly ICustomerDAL _customerDal;

        public CustomerService(ICustomerDAL customerDal)
        {
            _customerDal = customerDal;
        }

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerDal.GetAllCustomersAsync();
            return customers.Select(CustomerMapping.ToDto).ToList();
        }

        public async Task<CustomerDto?> GetByIdentityIdAsync(string identityId)
        {
            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId)) return null;

            var customer = await _customerDal.GetCustomerByIdentityIdAsync(identityId);
            return customer is null ? null : CustomerMapping.ToDto(customer);
        }

        public async Task<IReadOnlyList<CustomerDto>> GetByIdentityIdTypeAsync(IdentityIdType identityIdType)
        {
            var customers = await _customerDal.GetCustomersByIdentityIdTypeAsync((int)identityIdType);
            return customers.Select(CustomerMapping.ToDto).ToList();
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);
            var entity = CustomerMapping.ToEntity(normalized);

            try
            {
                await _customerDal.AddCustomerAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Customer with IdentityId '{normalized.IdentityId}' already exists or insert failed.", ex);
            }

            return CustomerMapping.ToDto(entity);
        }

        public async Task<CustomerDto> UpdateAsync(string identityId, UpdateCustomerRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId))
                throw new ArgumentException("IdentityId is required.", nameof(identityId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var entity = CustomerMapping.ToUpdateEntity(identityId, normalized);

            var updated = await _customerDal.UpdateCustomerAsync(entity);
            if (!updated)
                throw new KeyNotFoundException($"Customer with IdentityId '{identityId}' was not found.");

            return CustomerMapping.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string identityId)
        {
            identityId = NormalizeId(identityId);
            if (string.IsNullOrWhiteSpace(identityId)) return false;

            return await _customerDal.DeleteCustomerAsync(identityId);            
        }

        // -------------------------
        // Minimal normalization/validation
        // -------------------------

        private static string NormalizeId(string id) => (id ?? string.Empty).Trim();

        private static CreateCustomerRequest NormalizeCreate(CreateCustomerRequest r) => new()
        {
            IdentityId = (r.IdentityId ?? string.Empty).Trim(),
            IdentityIdType = r.IdentityIdType,
            MobileNo = (r.MobileNo ?? string.Empty).Trim(),
            Name = (r.Name ?? string.Empty).Trim(),
            Gender = r.Gender,
            Address = (r.Address ?? string.Empty).Trim(),
            Country = (r.Country ?? string.Empty).Trim()
        };

        private static UpdateCustomerRequest NormalizeUpdate(UpdateCustomerRequest r) => new()
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