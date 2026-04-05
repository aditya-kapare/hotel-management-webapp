using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICabDriverDAL
    {
        Task<IEnumerable<CabDriver>> GetAllDriversAsync();
        Task<CabDriver?> GetDriverByIdAsync(int driverId);
        Task AddDriverAsync(CabDriver driver);
        Task UpdateDriverAsync(CabDriver driver);
        Task DeleteDriverAsync(int driverId);
    }
}