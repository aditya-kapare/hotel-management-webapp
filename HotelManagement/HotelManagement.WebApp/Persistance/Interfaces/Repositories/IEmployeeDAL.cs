using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IEmployeeDAL
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByAadharAsync(string aadharNo);
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(string aadharNo);
    }
}