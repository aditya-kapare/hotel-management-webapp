using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Customers
{
    internal static class CustomerMapping
    {
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

        internal static Customer ToEntity(CreateCustomerRequest r) => new()
        {
            IdentityId = r.IdentityId,
            IdentityIdType = r.IdentityIdType,
            MobileNo = r.MobileNo,
            Name = r.Name,
            Gender = r.Gender,
            Address = r.Address,
            Country = r.Country
        };

        internal static void Apply(UpdateCustomerRequest r, Customer c)
        {
            c.MobileNo = r.MobileNo;
            c.Name = r.Name;
            c.Gender = r.Gender;
            c.Address = r.Address;
            c.Country = r.Country;
        }
    }
}