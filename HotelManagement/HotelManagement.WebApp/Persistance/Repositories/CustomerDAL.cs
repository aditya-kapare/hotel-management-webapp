using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HotelManagementSystem.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly HotelDbContext _context;

        public CustomerDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdentityIdAsync(string identityId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.IdentityId == identityId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByIdentityIdTypeAsync(int identityIdType)
        {
            return await _context.Customers
                .Where(c => (int)c.IdentityIdType == identityIdType)
                .ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string identityId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.IdentityId == identityId);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}