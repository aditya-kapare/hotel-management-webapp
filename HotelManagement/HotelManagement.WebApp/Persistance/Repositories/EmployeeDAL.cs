using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(string aadharNo)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.AadharNo == aadharNo);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}