using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Employees;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.WebApp.Application.Services
{
    /// <summary>
    /// Service handling employee-related business operations.
    /// </summary>
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDAL _employeeDal;
        private readonly UserManager<ApplicationEmployee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public EmployeeService(
            IEmployeeDAL employeeDal,
            UserManager<ApplicationEmployee> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            _employeeDal = employeeDal;
            _userManager = userManager;
            _roleManager = roleManager;
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

            // Determine if employee requires login access
            bool needsLogin =
                (normalized.EmployeePosition?.Contains("Admin", StringComparison.OrdinalIgnoreCase) == true) ||
                (normalized.EmployeePosition?.Contains("Reception", StringComparison.OrdinalIgnoreCase) == true);

            var entity = EmployeeMapping.ToEntity(normalized);

            bool created;

            if (needsLogin)
            {
                // Create identity user with password
                if (string.IsNullOrWhiteSpace(normalized.Password))
                    throw new ArgumentException("Password is required for Admin/Receptionist.");

                created = await _employeeDal.AddEmployeeAsync(entity, normalized.Password);

                if (!created)
                    throw new InvalidOperationException("User creation failed.");

                // Assign role based on employee position
                var roleName = normalized.EmployeePosition!.Contains("Admin", StringComparison.OrdinalIgnoreCase)
                    ? "Admin"
                    : "Receptionist";

                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new IdentityRole(roleName));

                var user = await _userManager.FindByEmailAsync(entity.EmailId);
                await _userManager.AddToRoleAsync(user!, roleName);
            }
            else
            {
                // Create employee without login access
                created = await _employeeDal.AddEmployeeAsync(entity, password: null);

                if (!created)
                    throw new InvalidOperationException("Employee creation failed.");
            }

            return EmployeeMapping.ToDetailsDto(entity);
        }

        public async Task<EmployeeDetailsDto> UpdateAsync(string aadharNo, UpdateEmployeeRequest request)
        {
            // Validate update request
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

            // Apply updated values to existing entity
            EmployeeMapping.Apply(existing, normalized);

            await _employeeDal.UpdateEmployeeAsync(existing);

            return EmployeeMapping.ToDetailsDto(existing);
        }


        public async Task<bool> DeleteAsync(string aadharNo)
        {
            // Normalize Aadhaar number before deletion
            aadharNo = NormalizeAadhar(aadharNo);
            if (string.IsNullOrWhiteSpace(aadharNo)) return false;

            return await _employeeDal.DeleteEmployeeByAadharAsync(aadharNo);
        }


        private static string NormalizeAadhar(string aadharNo)
            => (aadharNo ?? string.Empty).Trim();


        private static CreateEmployeeRequest NormalizeCreate(CreateEmployeeRequest r) => new()
        {
            // Trim all input fields
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

        /// <summary>
        /// Normalizes update employee request values.
        /// </summary>
        private static UpdateEmployeeRequest NormalizeUpdate(UpdateEmployeeRequest r) => new()
        {
            // Trim all updatable fields
            Name = r.Name?.Trim(),
            Age = r.Age,
            Gender = r.Gender,
            EmployeePosition = r.EmployeePosition?.Trim(),
            Salary = r.Salary,
            MobileNo = r.MobileNo?.Trim(),
            EmailId = r.EmailId?.Trim()
        };

        /// <summary>
        /// Validates create employee request.
        /// </summary>
        private static void ValidateCreate(CreateEmployeeRequest r)
        {
            // Enforce required fields for creation
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
