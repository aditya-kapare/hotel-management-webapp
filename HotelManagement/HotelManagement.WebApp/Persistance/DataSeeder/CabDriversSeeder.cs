using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class CabDriversSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            // Idempotent check
            if (await db.CabDrivers.AnyAsync(cancellationToken))
                return;

            var drivers = new List<CabDriver>
            {
                new CabDriver
                {
                    Name = "Ramesh Jadhav",
                    Age = 35,
                    Gender = Gender.Male,
                    CarVendor = "Ola",
                    CarType = "Sedan"
                },
                new CabDriver
                {
                    Name = "Suresh Patil",
                    Age = 42,
                    Gender = Gender.Male,
                    CarVendor = "Uber",
                    CarType = "SUV"
                },
                new CabDriver
                {
                    Name = "Amit Verma",
                    Age = 30,
                    Gender = Gender.Male,
                    CarVendor = "Private",
                    CarType = "Hatchback"
                },
                new CabDriver
                {
                    Name = "Sunita Desai",
                    Age = 38,
                    Gender = Gender.Female,
                    CarVendor = "Ola",
                    CarType = "Sedan"
                },
                new CabDriver
                {
                    Name = "Karan Singh",
                    Age = 45,
                    Gender = Gender.Male,
                    CarVendor = "Uber",
                    CarType = "SUV"
                },
                new CabDriver
                {
                    Name = "Neha Kulkarni",
                    Age = 33,
                    Gender = Gender.Female,
                    CarVendor = "Private",
                    CarType = "Sedan"
                }
            };

            await db.CabDrivers.AddRangeAsync(drivers, cancellationToken);

            // Do NOT call SaveChangesAsync here
            // SeedRunner handles it once after all seeders
        }
    }
}