using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IReadOnlyList<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdentityIdAsync(string identityId);
        Task<IReadOnlyList<CustomerDto>> GetByIdentityIdTypeAsync(IdentityIdType identityIdType);
        Task<CustomerDto> CreateAsync(CreateCustomerRequest request);
        Task<CustomerDto> UpdateAsync(string identityId, UpdateCustomerRequest request);
        Task<bool> DeleteAsync(string identityId);
    }
}