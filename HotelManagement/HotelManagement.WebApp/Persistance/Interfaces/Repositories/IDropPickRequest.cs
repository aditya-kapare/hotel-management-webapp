using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IDropPickRequestDAL
    {
        IEnumerable<DropPickRequest> GetAllRequests();
        DropPickRequest GetRequestById(int requestId);
        IEnumerable<DropPickRequest> GetRequestsByStayId(int stayId);
        IEnumerable<DropPickRequest> GetRequestsByDriverId(int driverId);
        void AddRequest(DropPickRequest request);
        void UpdateRequest(DropPickRequest request);
        void DeleteRequest(int requestId);
    }
}