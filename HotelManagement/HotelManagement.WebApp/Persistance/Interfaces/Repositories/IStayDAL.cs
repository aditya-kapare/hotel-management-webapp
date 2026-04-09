using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IStayDAL
    {
        Task<IEnumerable<Stay>> GetAllStaysAsync();
        Task<Stay?> GetStayByIdAsync(int stayId);
        Task<IEnumerable<Stay>> GetStaysByRoomNoAsync(int roomNo);
        Task<IEnumerable<Stay>> GetStaysByCustomerIdentityIdAsync(string customerIdentityId);
        Task<IEnumerable<Stay>> GetStaysByCheckInDateAsync(DateTime date);
        Task<bool> AddStayAsync(Stay stay);
        Task<bool> UpdateStayAsync(Stay stay);
        Task<bool> DeleteStayAsync(int stayId);
    }
}