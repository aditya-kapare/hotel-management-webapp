using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.DbContext;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing employee accounts.
    /// </summary>
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly AuthDbContext _authContext;
        private readonly UserManager<ApplicationEmployee> _userManager;

        /// <summary>
        /// Initializes the DAL with authentication context and user manager.
        /// </summary>
        public EmployeeDAL(
            AuthDbContext authContext,
            UserManager<ApplicationEmployee> userManager)
        {
            // Assign dependencies
            _authContext = authContext;
            _userManager = userManager;
        }

        
        public async Task<IEnumerable<ApplicationEmployee>> GetAllEmployeesAsync()
        {
            // Fetch all employees from identity store
            return await _userManager.Users.ToListAsync();
        }

  
        public async Task<ApplicationEmployee?> GetEmployeeByAadharAsync(string aadharNo)
        {
            // Find employee matching Aadhaar number
            return await _userManager.Users
                .FirstOrDefaultAsync(e => e.AadharNo == aadharNo);
        }

        /// <summary>
        /// Adds a new employee with optional password.
        /// </summary>
        public async Task<bool> AddEmployeeAsync(
            ApplicationEmployee employee,
            string? password)
        {
            // Create identity user with provided or generated password
            IdentityResult result;

            if (!string.IsNullOrWhiteSpace(password))
            {
                result = await _userManager.CreateAsync(employee, password);
                return result.Succeeded;
            }

            var randomPwd = "Tmp@" + Guid.NewGuid().ToString("N") + "aA1!";
            result = await _userManager.CreateAsync(employee, randomPwd);

            if (!result.Succeeded)
                return false;

            // Lock account when password is system-generated
            employee.LockoutEnabled = true;
            employee.LockoutEnd = DateTimeOffset.MaxValue;

            var update = await _userManager.UpdateAsync(employee);
            return update.Succeeded;
        }

        public async Task<bool> UpdateEmployeeAsync(ApplicationEmployee employee)
        {
            var result = await _userManager.UpdateAsync(employee);
            return result.Succeeded;
        }

     
        public async Task<bool> DeleteEmployeeByAadharAsync(string aadharNo)
        {
            // Locate employee by Aadhaar number
            var user = await _userManager.Users
                .FirstOrDefaultAsync(e => e.AadharNo == aadharNo);

            if (user == null)
                return false;

            // Remove employee from identity store
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}