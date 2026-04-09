using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly HotelDbContext _context;

        public EmployeeDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByAadharAsync(string aadharNo)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.AadharNo == aadharNo);
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> DeleteEmployeeAsync(string aadharNo)
        {
            var affected = await _context.Employees
                .Where(e => e.AadharNo == aadharNo)
                .ExecuteDeleteAsync();

            return affected > 0;
        }      
    }
}