using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IDropPickRequestService
    {
        Task<IReadOnlyList<DropPickRequestDto>> GetAllAsync();
        Task<DropPickRequestDto?> GetByIdAsync(int requestId);

        Task<IReadOnlyList<DropPickRequestDto>> GetByStayIdAsync(int stayId);
        Task<IReadOnlyList<DropPickRequestDto>> GetByDriverIdAsync(int driverId);

        Task<IReadOnlyList<CabDriver>> GetAvailableDriversAsync();

        Task<DropPickRequestDto> CreateAsync(CreateDropPickRequest request);
        Task<DropPickRequestDto> UpdateAsync(int requestId, UpdateDropPickRequest request);

        Task<bool> DeleteAsync(int requestId);
    }
}