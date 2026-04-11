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

        internal static ApplicationEmployee Apply(string aadharNo, UpdateEmployeeRequest r)
        {
            return new ApplicationEmployee
            {
                AadharNo = aadharNo,
                Name = r.Name,
                Age = r.Age,
                Gender = r.Gender,
                EmployeePosition = r.EmployeePosition,
                Salary = r.Salary,
                MobileNo = r.MobileNo,
                EmailId = r.EmailId,
                UserName = r.EmailId ?? aadharNo
            };
        }
    }
}