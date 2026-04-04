using HotelManagement.WebApp.Domain.Models;
using System.Collections.Generic;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICustomerDAL
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerByIdentityId(string identityId);
        IEnumerable<Customer> GetCustomersByIdentityIdType(int identityIdType);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(string identityId);
    }
}