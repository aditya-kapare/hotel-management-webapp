using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Employees
{
    internal static class EmployeeMapping
    {
        internal static EmployeeDetailsDto ToDetailsDto(Employee e) => new()
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

        internal static EmployeeSummaryDto ToSummaryDto(Employee e) => new()
        {
            AadharNo = e.AadharNo,
            Name = e.Name,
            MobileNo = e.MobileNo,
            EmployeePosition = e.EmployeePosition,
            Gender = e.Gender
        };

        internal static Employee ToEntity(CreateEmployeeRequest r) => new()
        {
            AadharNo = r.AadharNo,
            Name = r.Name,
            Age = r.Age,
            Gender = r.Gender,
            EmployeePosition = r.EmployeePosition,
            Salary = r.Salary,
            MobileNo = r.MobileNo,
            EmailId = r.EmailId
        };

        internal static void Apply(UpdateEmployeeRequest r, Employee e)
        {
            e.Name = r.Name;
            e.Age = r.Age;
            e.Gender = r.Gender;
            e.EmployeePosition = r.EmployeePosition;
            e.Salary = r.Salary;
            e.MobileNo = r.MobileNo;
            e.EmailId = r.EmailId;
        }
    }
}