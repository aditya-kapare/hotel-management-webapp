using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICustomerDAL
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdentityIdAsync(string identityId);
        Task<IEnumerable<Customer>> GetCustomersByIdentityIdTypeAsync(int identityIdType);
        Task<bool> AddCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(string identityId);
    }
}