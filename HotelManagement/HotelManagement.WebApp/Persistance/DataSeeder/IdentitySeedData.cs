using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public static class IdentitySeedData
    {
        private const string AdminPassword = "Admin@123";
        private const string ReceptionistPassword = "Reception@123";

        public static async Task PopulateAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<AuthDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationEmployee>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Receptionist"))
                await roleManager.CreateAsync(new IdentityRole("Receptionist"));

            var employees = new List<ApplicationEmployee>
            {
                new ApplicationEmployee
                {
                    AadharNo = "111122223333",
                    Name = "Rohit Sharma",
                    Age = 35,
                    Gender = Gender.Male,
                    EmployeePosition = "Hotel Manager",
                    Salary = 60000m,
                    PhoneNumber = "9876543211",
                    Email = "rohit.sharma@hotel.com",
                    UserName = "rohit.sharma@hotel.com",
                    EmailConfirmed = true
                },
                new ApplicationEmployee
                {
                    AadharNo = "222233334444",
                    Name = "Pooja Hegde",
                    Age = 29,
                    Gender = Gender.Female,
                    EmployeePosition = "Receptionist",
                    Salary = 28000m,
                    PhoneNumber = "9876543212",
                    Email = "pooja.hegde@hotel.com",
                    UserName = "pooja.hegde@hotel.com",
                    EmailConfirmed = true
                },
                new ApplicationEmployee
                {
                    AadharNo = "333344445555",
                    Name = "Sanjay Dutt",
                    Age = 41,
                    Gender = Gender.Male,
                    EmployeePosition = "Housekeeping Supervisor",
                    Salary = 32000m,
                    PhoneNumber = "9876543213",
                    Email = "sanjay.dutt@hotel.com",
                    UserName = "sanjay.dutt@hotel.com",
                    EmailConfirmed = true
                },
                new ApplicationEmployee
                {
                    AadharNo = "444455556666",
                    Name = "Virat Kohli",
                    Age = 26,
                    Gender = Gender.Female,
                    EmployeePosition = "Front Desk Executive",
                    Salary = 25000m,
                    PhoneNumber = "9876543214",
                    Email = "virat.kohli@hotel.com",
                    UserName = "virat.kohli@hotel.com",
                    EmailConfirmed = true
                },
                new ApplicationEmployee
                {
                    AadharNo = "555566667777",
                    Name = "Amit Bacchan",
                    Age = 38,
                    Gender = Gender.Male,
                    EmployeePosition = "Maintenance Staff",
                    Salary = 22000m,
                    PhoneNumber = "9876543215",
                    Email = null,
                    UserName = "555566667777",
                    EmailConfirmed = false
                },
                new ApplicationEmployee
                {
                    AadharNo = "666677778888",
                    Name = "Suleman Khan",
                    Age = 32,
                    Gender = Gender.Female,
                    EmployeePosition = "Accounts Executive",
                    Salary = 40000m,
                    PhoneNumber = "9876543216",
                    Email = "suleman.bhai@hotel.com",
                    UserName = "suleman.bhai@hotel.com",
                    EmailConfirmed = true
                }
            };

            foreach (var emp in employees)
            {
                
                var existing = await context.Users.SingleOrDefaultAsync(u => u.AadharNo == emp.AadharNo);
                if (existing != null) continue;

                var pos = (emp.EmployeePosition ?? string.Empty).ToLowerInvariant();

                var isAdmin = pos.Contains("manager");                 
                var isReceptionist = pos.Contains("receptionist");     

                IdentityResult result;

                if (isAdmin)
                {
                    result = await userManager.CreateAsync(emp, AdminPassword);
                    if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    await userManager.AddToRoleAsync(emp, "Admin");
                }
                else if (isReceptionist)
                {
                    result = await userManager.CreateAsync(emp, ReceptionistPassword);
                    if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    await userManager.AddToRoleAsync(emp, "Receptionist");
                }
                else
                {
                   
                    var randomPwd = "Tmp@" + Guid.NewGuid().ToString("N") + "aA1!";
                    result = await userManager.CreateAsync(emp, randomPwd);
                    if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                    emp.LockoutEnabled = true;
                    emp.LockoutEnd = DateTimeOffset.MaxValue;
                    await userManager.UpdateAsync(emp);
                }
            }
        }
    }
}