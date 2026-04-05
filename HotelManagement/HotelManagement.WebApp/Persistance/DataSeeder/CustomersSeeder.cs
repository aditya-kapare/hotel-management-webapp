using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class CustomersSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            // Idempotent seeding
            if (await db.Customers.AnyAsync(cancellationToken))
                return;

            var customers = new List<Customer>
            {
                new Customer
                {
                    IdentityId = "123456789012",
                    IdentityIdType = IdentityIdType.Aadhar,
                    Name = "Rahul Sharma",
                    Gender = Gender.Male,
                    MobileNo = "9876543210",
                    Address = "Baner, Pune",
                    Country = "India"
                },
                new Customer
                {
                    IdentityId = "987654321098",
                    IdentityIdType = IdentityIdType.Aadhar,
                    Name = "Sneha Patil",
                    Gender = Gender.Female,
                    MobileNo = "9123456780",
                    Address = "Kothrud, Pune",
                    Country = "India"
                },
                new Customer
                {
                    IdentityId = "P1234567",
                    IdentityIdType = IdentityIdType.Passport,
                    Name = "John Williams",
                    Gender = Gender.Male,
                    MobileNo = "9988776655",
                    Address = "New York",
                    Country = "USA"
                },
                new Customer
                {
                    IdentityId = "P9988776",
                    IdentityIdType = IdentityIdType.Passport,
                    Name = "Emily Clark",
                    Gender = Gender.Female,
                    MobileNo = "8899776655",
                    Address = "London",
                    Country = "UK"
                },
                new Customer
                {
                    IdentityId = "DL-MH12-445566",
                    IdentityIdType = IdentityIdType.DrivingLicense,
                    Name = "Amit Kulkarni",
                    Gender = Gender.Male,
                    MobileNo = "9011223344",
                    Address = "Aundh, Pune",
                    Country = "India"
                },
                new Customer
                {
                    IdentityId = "DL-KA09-778899",
                    IdentityIdType = IdentityIdType.DrivingLicense,
                    Name = "Ananya Rao",
                    Gender = Gender.Female,
                    MobileNo = "9345678901",
                    Address = "Bengaluru",
                    Country = "India"
                },
                new Customer
                {
                    IdentityId = "P5544332",
                    IdentityIdType = IdentityIdType.Passport,
                    Name = "Michael Brown",
                    Gender = Gender.Male,
                    MobileNo = "7766554433",
                    Address = "Sydney",
                    Country = "Australia"
                },
                new Customer
                {
                    IdentityId = "123443211234",
                    IdentityIdType = IdentityIdType.Aadhar,
                    Name = "Priya Deshmukh",
                    Gender = Gender.Female,
                    MobileNo = "9988112233",
                    Address = "Nagpur",
                    Country = "India"
                }
            };

            await db.Customers.AddRangeAsync(customers, cancellationToken);
        }
    }
}
