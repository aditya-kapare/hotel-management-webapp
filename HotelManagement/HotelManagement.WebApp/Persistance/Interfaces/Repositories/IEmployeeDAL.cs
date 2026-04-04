using HotelManagement.WebApp.Domain.Models;
using System.Collections.Generic;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IEmployeeDAL
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeByAadhar(string aadharNo);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(string aadharNo);
    }
}