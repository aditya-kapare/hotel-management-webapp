using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly HotelDbContext _context;

        public EmployeeDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeByAadhar(string aadharNo) 
            => _context.Employees.FirstOrDefault(e => e.AadharNo == aadharNo);

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(string aadharNo)
        {
            var employee = _context.Employees
                                   .FirstOrDefault(e => e.AadharNo == aadharNo);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }
    }
}

