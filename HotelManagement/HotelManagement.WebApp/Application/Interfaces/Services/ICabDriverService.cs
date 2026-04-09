using HotelManagement.WebApp.Application.Dtos.Drivers;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface ICabDriverService
    {
        Task<IReadOnlyList<CabDriverDto>> GetAllAsync();
        Task<CabDriverDto?> GetByIdAsync(int driverId);

        Task<CabDriverDto> CreateAsync(CabDriverRequest request);
        Task<CabDriverDto> UpdateAsync(int driverId, CabDriverRequest request);

        Task<bool> DeleteAsync(int driverId);
    }
}
