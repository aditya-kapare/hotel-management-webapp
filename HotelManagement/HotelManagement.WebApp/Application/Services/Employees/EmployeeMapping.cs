using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Employees
{
    internal static class EmployeeMapping
    {
        internal static EmployeeDetailsDto ToDetailsDto(ApplicationEmployee e) => new()
        {
            AadharNo = e.AadharNo,
            Name = e.Name,
            Age = e.Age,
            Gender = e.Gender,
            EmployeePosition = e.EmployeePosition,
            Salary = e.Salary,
            MobileNo = e.MobileNo,
            EmailId = e.EmailId
        };

        internal static EmployeeSummaryDto ToSummaryDto(ApplicationEmployee e) => new()
        {
            AadharNo = e.AadharNo,
            Name = e.Name,
            MobileNo = e.MobileNo,
            EmployeePosition = e.EmployeePosition,
            Gender = e.Gender
        };

        internal static ApplicationEmployee ToEntity(CreateEmployeeRequest r) => new()
        {
            AadharNo = r.AadharNo,
            Name = r.Name,
            Age = r.Age,
            Gender = r.Gender,
            EmployeePosition = r.EmployeePosition,
            Salary = r.Salary,
            MobileNo = r.MobileNo,
            EmailId = r.EmailId,

            // Identity-required
            UserName = r.EmailId ?? r.AadharNo,
            EmailConfirmed = true
        };

        internal static void Apply(ApplicationEmployee existing, UpdateEmployeeRequest r)
        {
            if (existing == null)
                throw new ArgumentNullException(nameof(existing));

            existing.Name = r.Name;
            existing.Age = r.Age;
            existing.Gender = r.Gender;
            existing.EmployeePosition = r.EmployeePosition;
            existing.Salary = r.Salary;
            existing.MobileNo = r.MobileNo;

            if (!string.IsNullOrWhiteSpace(r.EmailId) &&
                !string.Equals(existing.EmailId, r.EmailId, StringComparison.OrdinalIgnoreCase))
            {
                existing.EmailId = r.EmailId;
                existing.UserName = r.EmailId; // Only update when email actually changes
            }
        }
    }
}