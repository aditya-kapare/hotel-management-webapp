using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly HotelDbContext _context;

        public CustomerDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerByIdentityId(string identityId)
            => _context.Customers.FirstOrDefault(c => c.IdentityId == identityId);

        public IEnumerable<Customer> GetCustomersByIdentityIdType(int identityIdType)
        {
            return _context.Customers.Where(c => (int)c.IdentityIdType == identityIdType).ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(string identityId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.IdentityId == identityId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}