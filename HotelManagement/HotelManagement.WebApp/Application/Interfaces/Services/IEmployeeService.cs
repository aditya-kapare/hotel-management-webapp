using HotelManagement.WebApp.Application.Dtos.Employee;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeSummaryDto>> GetAllAsync();

        Task<EmployeeDetailsDto?> GetByAadharAsync(string aadharNo);

        Task<EmployeeDetailsDto> CreateAsync(CreateEmployeeRequest request);

        Task<EmployeeDetailsDto> UpdateAsync(string aadharNo, UpdateEmployeeRequest request);

        Task<bool> DeleteAsync(string aadharNo);
    }
}