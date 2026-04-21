using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing customer records.
    /// </summary>
    public class CustomerDAL : ICustomerDAL
    {
        private readonly HotelDbContext _context;

        public CustomerDAL(HotelDbContext context)
        {
           
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            // Fetch all customers from database
            return await _context.Customers.ToListAsync();
        }


        public async Task<Customer?> GetCustomerByIdentityIdAsync(string identityId)
        {
            // Find customer matching the identity ID
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.IdentityId == identityId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByIdentityIdTypeAsync(int identityIdType)
        {
            // Filter customers by identity ID type
            return await _context.Customers
                .Where(c => (int)c.IdentityIdType == identityIdType)
                .ToListAsync();
        }

       
        public async Task<bool> AddCustomerAsync(Customer customer)
        {
           
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
          
            _context.Customers.Update(customer);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrent update failure
                return false;
            }
        }

        /// <summary>
        /// Deletes a customer by identity ID.
        /// </summary>
        public async Task<bool> DeleteCustomerAsync(string identityId)
        {
            
            var customer = await _context.Customers
                .Where(c => c.IdentityId == identityId)
                .ExecuteDeleteAsync();

            return customer > 0;
        }
    }
}
