using HotelManagement.WebApp.Application.Dtos.Drivers;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface ICabDriverService
    {
        Task<IReadOnlyList<CabDriverDto>> GetAllAsync();
        Task<CabDriverDto?> GetByIdAsync(int driverId);

        Task<CabDriverDto> CreateAsync(CreateCabDriverRequest request);
        Task<CabDriverDto> UpdateAsync(int driverId, UpdateCabDriverRequest request);

        Task<bool> DeleteAsync(int driverId);
    }
}
