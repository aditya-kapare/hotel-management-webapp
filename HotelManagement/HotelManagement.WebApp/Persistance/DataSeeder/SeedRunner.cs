using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public static class SeedRunner
    {
        public static async Task RunAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            await db.Database.MigrateAsync(cancellationToken);

            var seeders = new ISeeder[]
            {
                new RoomsSeeder(),
                new CustomersSeeder(),
                new CabDriversSeeder(),

                new StaysSeeder(),
                new DropPickRequestsSeeder()
            };

            foreach (var seeder in seeders)
            {
                Console.WriteLine($"Running seeder: {seeder.GetType().Name}");
                await seeder.SeedAsync(db, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
            }

            Console.WriteLine("Seeding finished.");
        }
    }
}
