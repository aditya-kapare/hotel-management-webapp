using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICustomerDAL
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdentityIdAsync(string identityId);
        Task<IEnumerable<Customer>> GetCustomersByIdentityIdTypeAsync(int identityIdType);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string identityId);
    }
}