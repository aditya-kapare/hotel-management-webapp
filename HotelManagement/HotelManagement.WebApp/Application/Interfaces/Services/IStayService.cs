using HotelManagement.WebApp.Application.Dtos.Billing;
using HotelManagement.WebApp.Application.Dtos.Stays;

namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IStayService
    {
        Task<IReadOnlyList<StayDto>> GetAllAsync();
        Task<IReadOnlyList<StayDto>> GetActiveAsync();
        Task<StayDto?> GetByIdAsync(int stayId);

        Task<IReadOnlyList<StayDto>> GetByRoomNoAsync(int roomNo);
        Task<IReadOnlyList<StayDto>> GetByCustomerIdentityIdAsync(string customerIdentityId);
        Task<IReadOnlyList<StayDto>> GetByCheckInDateAsync(DateTime date);

        Task<StayDto> CheckInAsync(CheckInRequest request);                
        Task<StayDto> UpdateAsync(int stayId, UpdateStayRequest request);   
        Task<StayDto> CheckOutAsync(int stayId, CheckOutRequest request);
        Task<BillingSummaryDto> GetBillingSummaryAsync(int stayId);

        Task<bool> DeleteAsync(int stayId);
    }
}