using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Employees;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

            if (string.IsNullOrWhiteSpace(aadharNo))
                return null;

            var employee = await _employeeDal.GetEmployeeByAadharAsync(aadharNo); 

            return employee is null ? null : EmployeeMapping.ToDetailsDto(employee);
        }

        public async Task<EmployeeDetailsDto> CreateAsync(CreateEmployeeRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);

            ValidateCreate(normalized);

            Employee entity = EmployeeMapping.ToEntity(normalized);


            try
            {
                await _employeeDal.AddEmployeeAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                throw 
                    new InvalidOperationException($"Employee with Aadhar '{normalized.AadharNo}' already exists or insert failed.", ex);
            }

            return EmployeeMapping.ToDetailsDto(entity);
        }

        public async Task<EmployeeDetailsDto> UpdateAsync(string aadharNo, UpdateEmployeeRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            aadharNo = NormalizeAadhar(aadharNo);
            if (string.IsNullOrWhiteSpace(aadharNo))
            {
                throw new ArgumentException("AadharNo is required.", nameof(aadharNo));
            }

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var entity = EmployeeMapping.Apply(aadharNo, normalized);

            var updated = await _employeeDal.UpdateEmployeeAsync(entity);
            if (!updated) throw new KeyNotFoundException($"Employee with Aadhar '{aadharNo}' was not found.");

            return EmployeeMapping.ToDetailsDto(entity);
        }

        public async Task<bool> DeleteAsync(string aadharNo)
        {
            aadharNo = NormalizeAadhar(aadharNo);            
            if (string.IsNullOrWhiteSpace(aadharNo)) return false;

            return await _employeeDal.DeleteEmployeeAsync(aadharNo); 
        }

        // Minimal validation + normalize
        private static string NormalizeAadhar(string aadharNo) 
            => (aadharNo ?? string.Empty).Trim();

        private static CreateEmployeeRequest NormalizeCreate(CreateEmployeeRequest r)
            => new()
            {
                AadharNo = (r.AadharNo ?? string.Empty).Trim(),
                Name = (r.Name ?? string.Empty).Trim(),
                Age = r.Age,
                Gender = r.Gender,
                EmployeePosition = string.IsNullOrWhiteSpace(r.EmployeePosition) ? null : r.EmployeePosition.Trim(),
                Salary = r.Salary,
                MobileNo = (r.MobileNo ?? string.Empty).Trim(),
                EmailId = string.IsNullOrWhiteSpace(r.EmailId) ? null : r.EmailId.Trim()
            };

        private static UpdateEmployeeRequest NormalizeUpdate(UpdateEmployeeRequest r)
            => new()
            {
                Name = (r.Name ?? string.Empty).Trim(),
                Age = r.Age,
                Gender = r.Gender,
                EmployeePosition = string.IsNullOrWhiteSpace(r.EmployeePosition) ? null : r.EmployeePosition.Trim(),
                Salary = r.Salary,
                MobileNo = (r.MobileNo ?? string.Empty).Trim(),
                EmailId = string.IsNullOrWhiteSpace(r.EmailId) ? null : r.EmailId.Trim()
            };

        private static void ValidateCreate(CreateEmployeeRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.AadharNo))
                throw new ArgumentException("AadharNo is required.", nameof(r.AadharNo));

            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.", nameof(r.Name));

            if (string.IsNullOrWhiteSpace(r.MobileNo))
                throw new ArgumentException("MobileNo is required.", nameof(r.MobileNo));

            if (r.Age < 0)
                throw new ArgumentException("Age cannot be negative.", nameof(r.Age));

            if (r.Salary < 0)
                throw new ArgumentException("Salary cannot be negative.", nameof(r.Salary));
        }

        private static void ValidateUpdate(UpdateEmployeeRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.", nameof(r.Name));

            if (string.IsNullOrWhiteSpace(r.MobileNo))
                throw new ArgumentException("MobileNo is required.", nameof(r.MobileNo));

            if (r.Age < 0)
                throw new ArgumentException("Age cannot be negative.", nameof(r.Age));

            if (r.Salary < 0)
                throw new ArgumentException("Salary cannot be negative.", nameof(r.Salary));
        }
    }
}