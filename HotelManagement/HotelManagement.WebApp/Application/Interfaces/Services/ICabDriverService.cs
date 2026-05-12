using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagementSystem.DAL;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface ICabDriverService
    {

        // READ
        Task<IReadOnlyList<CabDriverDTO>> GetAllAsync();
        Task<CabDriverDTO?> GetByIdAsync(int driverId);
        Task<CabDriverDTO?> GetByGovtIdAsync(string govtId);

        // CREATE
        Task<CabDriverDTO> CreateAsync(CabDriverForCreationDTO request);

        // UPDATE
        Task<CabDriverDTO> UpdateByIdAsync(int driverId, CabDriverForUpdateDTO request);
        Task<CabDriverDTO> UpdateByGovtIdAsync(string govtId, CabDriverForUpdateDTO request);

        // DELETE
        Task<bool> DeleteByIdAsync(int driverId);
        Task<bool> DeleteByGovtIdAsync(string govtId);

    }
}
