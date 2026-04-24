using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Customers
{
    /// <summary>
    /// Provides mapping logic between customer entities and DTOs.
    /// </summary>
    internal static class CustomerMapping
    {
        /// <summary>
        /// Maps a customer entity to a DTO.
        /// </summary>
        internal static CustomerDto ToDto(Customer c) => new()
        {

            IdentityId = c.IdentityId,
            IdentityIdType = c.IdentityIdType,
            MobileNo = c.MobileNo,
            Name = c.Name,
            Gender = c.Gender,
            Address = c.Address,
            Country = c.Country
        };

        /// <summary>
        /// Maps a create request to a customer entity.
        /// </summary>
        internal static Customer ToEntity(CreateCustomerRequest r) => new()
        {
            // Create new customer entity from request
            IdentityId = r.IdentityId,
            IdentityIdType = r.IdentityIdType,
            MobileNo = r.MobileNo,
            Name = r.Name,
            Gender = r.Gender,
            Address = r.Address,
            Country = r.Country
        };

        /// <summary>
        /// Creates a customer entity for update operations.
        /// </summary>
        internal static Customer ToUpdateEntity(string identityId, UpdateCustomerRequest r) => new()
        {
            // Prepare entity with updated values
            IdentityId = identityId,
            MobileNo = r.MobileNo,
            Name = r.Name,
            Gender = r.Gender,
            Address = r.Address,
            Country = r.Country
        };

        /// <summary>
        /// Applies update request values to an existing customer entity.
        /// </summary>
        internal static void Apply(UpdateCustomerRequest r, Customer c)
        {
            // Update mutable customer fields
            c.MobileNo = r.MobileNo;
            c.Name = r.Name;
            c.Gender = r.Gender;
            c.Address = r.Address;
            c.Country = r.Country;
        }
    }
}