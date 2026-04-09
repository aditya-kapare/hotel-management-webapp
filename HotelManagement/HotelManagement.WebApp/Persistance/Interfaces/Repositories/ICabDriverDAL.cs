using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface ICabDriverDAL
    {
        Task<IEnumerable<CabDriver>> GetAllDriversAsync();
        Task<CabDriver?> GetDriverByIdAsync(int driverId);
        Task<bool> AddDriverAsync(CabDriver driver);
        Task<bool> UpdateDriverAsync(CabDriver driver);
        Task<bool> DeleteDriverAsync(int driverId);
    }
}