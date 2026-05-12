using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.ViewModels.DropPickRequests;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IDropPickRequestService
    {
        Task<IReadOnlyList<DropPickRequestDto>> GetAllAsync();
        Task<IReadOnlyList<DropPickRequestDto>> GetRequestListAsync();
        Task<IReadOnlyList<DropPickRequestDto>> GetOngoingListAsync();
        Task<IReadOnlyList<DropPickRequestDto>> GetPastListAsync();
        Task<DropPickRequestDto?> GetByIdAsync(int requestId);

        Task<IReadOnlyList<DropPickRequestDto>> GetByStayIdAsync(int stayId);
        Task<IReadOnlyList<DropPickRequestDto>> GetByDriverIdAsync(int driverId);

        Task<IReadOnlyList<CabDriverBriefDto>> GetAvailableDriversAsync();

        Task<DropPickRequestDto> CreateAsync(CreateDropPickRequest request);
        Task<DropPickRequest> CreateAsync(DropPickRequest request);
        Task<DropPickRequestDto> UpdateAsync(int requestId, UpdateDropPickRequest request);
        Task<DropPickRequestDto?> GetRequestByIdAsync(int requestId);
        Task<bool> DeleteAsync(int requestId);
    }
}