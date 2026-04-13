using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IDropPickRequestDAL
    {
        Task<IEnumerable<DropPickRequest>> GetAllRequestsAsync();
        Task<DropPickRequest?> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<DropPickRequest>> GetRequestsByStayIdAsync(int stayId);
        Task<IEnumerable<DropPickRequest>> GetRequestsByDriverIdAsync(int driverId);
        Task<bool> AddRequestAsync(DropPickRequest request);
        Task<bool> UpdateRequestAsync(DropPickRequest request);
        Task<bool> DeleteRequestAsync(int requestId);
        Task<IEnumerable<CabDriver>> GetAvailableDriversAsync();
    }
}