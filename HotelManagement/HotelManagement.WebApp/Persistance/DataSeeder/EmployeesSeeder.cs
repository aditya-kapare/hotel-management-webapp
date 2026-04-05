using global::HotelManagement.WebApp.Domain.Enums;
using global::HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class EmployeesSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            // Idempotent check
            if (await db.Employees.AnyAsync(cancellationToken))
                return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    AadharNo = "111122223333",
                    Name = "Rohit Sharma",
                    Age = 35,
                    Gender = Gender.Male,
                    EmployeePosition = "Hotel Manager",
                    Salary = 60000m,
                    MobileNo = "9876543211",
                    EmailId = "rohit.sharma@hotel.com"
                },
                new Employee
                {
                    AadharNo = "222233334444",
                    Name = "Pooja Hegde",
                    Age = 29,
                    Gender = Gender.Female,
                    EmployeePosition = "Receptionist",
                    Salary = 28000m,
                    MobileNo = "9876543212",
                    EmailId = "pooja.hegde@hotel.com"
                },
                new Employee
                {
                    AadharNo = "333344445555",
                    Name = "Sanjay Dutt",
                    Age = 41,
                    Gender = Gender.Male,
                    EmployeePosition = "Housekeeping Supervisor",
                    Salary = 32000m,
                    MobileNo = "9876543213",
                    EmailId = "sanjay.dutt@hotel.com"
                },
                new Employee
                {
                    AadharNo = "444455556666",
                    Name = "Virat Kohli",
                    Age = 26,
                    Gender = Gender.Female,
                    EmployeePosition = "Front Desk Executive",
                    Salary = 25000m,
                    MobileNo = "9876543214",
                    EmailId = "virat.kohli@hotel.com"
                },
                new Employee
                {
                    AadharNo = "555566667777",
                    Name = "Amit Bacchan",
                    Age = 38,
                    Gender = Gender.Male,
                    EmployeePosition = "Maintenance Staff",
                    Salary = 22000m,
                    MobileNo = "9876543215",
                    EmailId = null
                },
                new Employee
                {
                    AadharNo = "666677778888",
                    Name = "Suleman Khan",
                    Age = 32,
                    Gender = Gender.Female,
                    EmployeePosition = "Accounts Executive",
                    Salary = 40000m,
                    MobileNo = "9876543216",
                    EmailId = "suleman.bhai@hotel.com"
                }
            };

            await db.Employees.AddRangeAsync(employees, cancellationToken);
        }
    }
}