using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IEmployeeDAL
    {
        Task<IEnumerable<ApplicationEmployee>> GetAllEmployeesAsync();
        Task<ApplicationEmployee?> GetEmployeeByAadharAsync(string aadharNo);
        Task<bool> AddEmployeeAsync(ApplicationEmployee employee, string? password);
        Task<bool> UpdateEmployeeAsync(ApplicationEmployee employee);
        Task<bool> DeleteEmployeeByAadharAsync(string aadharNo);
    }
}