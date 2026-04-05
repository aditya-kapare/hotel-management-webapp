using HotelManagement.WebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IStayDAL
    {
        Task<IEnumerable<Stay>> GetAllStaysAsync();
        Task<Stay?> GetStayByIdAsync(int stayId);
        Task<IEnumerable<Stay>> GetStaysByRoomNoAsync(int roomNo);
        Task<IEnumerable<Stay>> GetStaysByCustomerIdentityIdAsync(string customerIdentityId);
        Task<IEnumerable<Stay>> GetStaysByCheckInDateAsync(DateTime date);
        Task AddStayAsync(Stay stay);
        Task UpdateStayAsync(Stay stay);
        Task DeleteStayAsync(int stayId);
    }
}