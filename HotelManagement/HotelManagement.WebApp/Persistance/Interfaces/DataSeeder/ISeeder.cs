using HotelManagementSystem.Data;

namespace HotelManagement.WebApp.Persistance.Interfaces.DataSeeder
{
    public interface ISeeder
    {
        Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default);
    }
}