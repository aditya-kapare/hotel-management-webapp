using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Employees;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDAL _employeeDal;

        public EmployeeService(IEmployeeDAL employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public async Task<IReadOnlyList<EmployeeSummaryDto>> GetAllAsync()
        {
            var employees = await _employeeDal.GetAllEmployeesAsync();
            return employees.Select(EmployeeMapping.ToSummaryDto).ToList();
        }

        public async Task<EmployeeDetailsDto?> GetByAadharAsync(string aadharNo)
        {
            aadharNo = NormalizeAadhar(aadharNo);
            if (string.IsNullOrWhiteSpace(aadharNo)) return null;

            var employee = await _employeeDal.GetEmployeeByAadharAsync(aadharNo);
            return employee is null ? null : EmployeeMapping.ToDetailsDto(employee);
        }

        public async Task<EmployeeDetailsDto> CreateAsync(CreateEmployeeRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);
            bool needsLogin =
                (normalized.EmployeePosition?.Contains("Admin", StringComparison.OrdinalIgnoreCase) == true) ||
                (normalized.EmployeePosition?.Contains("Reception", StringComparison.OrdinalIgnoreCase) == true);

            var entity = EmployeeMapping.ToEntity(normalized);

            bool created;

            if (needsLogin)
            {
                if (string.IsNullOrWhiteSpace(normalized.Password))
                    throw new ArgumentException("Password is required for Admin/Receptionist.");

                created = await _employeeDal.AddEmployeeAsync(entity, normalized.Password);
            }
            else
            {
                created = await _employeeDal.AddEmployeeAsync(entity, password: null);
            }

            if (!created)
                throw new InvalidOperationException($"Employee with Aadhar '{normalized.AadharNo}' could not be created.");

            return EmployeeMapping.ToDetailsDto(entity);
        }

        public async Task<EmployeeDetailsDto> UpdateAsync( string aadharNo, UpdateEmployeeRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            aadharNo = NormalizeAadhar(aadharNo);
            if (string.IsNullOrWhiteSpace(aadharNo))
                throw new ArgumentException("AadharNo is required.", nameof(aadharNo));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var existing = await _employeeDal.GetEmployeeByAadharAsync(aadharNo);
            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Employee with Aadhar '{aadharNo}' was not found.");
            }

            EmployeeMapping.Apply(existing, normalized);

            await _employeeDal.UpdateEmployeeAsync(existing);

            return EmployeeMapping.ToDetailsDto(existing);
        }

        public async Task<bool> DeleteAsync(string aadharNo)
        {
            aadharNo = NormalizeAadhar(aadharNo);
            if (string.IsNullOrWhiteSpace(aadharNo)) return false;

            return await _employeeDal.DeleteEmployeeByAadharAsync(aadharNo);
        }

        private static string NormalizeAadhar(string aadharNo)
            => (aadharNo ?? string.Empty).Trim();

        private static CreateEmployeeRequest NormalizeCreate(CreateEmployeeRequest r) => new()
        {
            AadharNo = r.AadharNo?.Trim(),
            Name = r.Name?.Trim(),
            Age = r.Age,
            Gender = r.Gender,
            EmployeePosition = r.EmployeePosition?.Trim(),
            Salary = r.Salary,
            MobileNo = r.MobileNo?.Trim(),
            EmailId = r.EmailId?.Trim(),
            Password = r.Password
        };

        private static UpdateEmployeeRequest NormalizeUpdate(UpdateEmployeeRequest r) => new()
        {
            Name = r.Name?.Trim(),
            Age = r.Age,
            Gender = r.Gender,
            EmployeePosition = r.EmployeePosition?.Trim(),
            Salary = r.Salary,
            MobileNo = r.MobileNo?.Trim(),
            EmailId = r.EmailId?.Trim()
        };

        private static void ValidateCreate(CreateEmployeeRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.AadharNo)) throw new ArgumentException("AadharNo is required");
            if (string.IsNullOrWhiteSpace(r.Name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrWhiteSpace(r.MobileNo)) throw new ArgumentException("MobileNo is required");
            if (string.IsNullOrWhiteSpace(r.Password)) throw new ArgumentException("Password is required");
        }

        private static void ValidateUpdate(UpdateEmployeeRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrWhiteSpace(r.MobileNo)) throw new ArgumentException("MobileNo is required");
        }
    }
}