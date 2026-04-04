using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Repositories
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
