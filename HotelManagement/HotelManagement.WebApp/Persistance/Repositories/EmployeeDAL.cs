using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.DbContext;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly AuthDbContext _authContext;
        private readonly UserManager<ApplicationEmployee> _userManager;

        public EmployeeDAL(AuthDbContext authContext, UserManager<ApplicationEmployee> userManager)
        {
            _authContext = authContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationEmployee>> GetAllEmployeesAsync()
        {
            return await _authContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationEmployee?> GetEmployeeByAadharAsync(string aadharNo)
        {
            return await _authContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.AadharNo == aadharNo);
        }

        public async Task<bool> AddEmployeeAsync(ApplicationEmployee employee, string? password)
        {
            IdentityResult result;

            if (!string.IsNullOrWhiteSpace(password))
            {
                result = await _userManager.CreateAsync(employee, password);
                return result.Succeeded;
            }

            var randomPwd = "Tmp@" + Guid.NewGuid().ToString("N") + "aA1!";
            result = await _userManager.CreateAsync(employee, randomPwd);
            if (!result.Succeeded) return false;

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
            var user = await _authContext.Users.FirstOrDefaultAsync(e => e.AadharNo == aadharNo);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}