using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.DAL

{
    public class DropPickRequestDAL : IDropPickRequestDAL
    {
        private readonly HotelDbContext _context;

        public DropPickRequestDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DropPickRequest> GetAllRequests()
        {
            return _context.DropPickRequests.ToList();
        }

        public DropPickRequest GetRequestById(int requestId)
            => _context.DropPickRequests.FirstOrDefault(r => r.RequestId == requestId);

        public IEnumerable<DropPickRequest> GetRequestsByStayId(int stayId)
        {
            return _context.DropPickRequests.Where(r => r.StayId == stayId).ToList();
        }

        public IEnumerable<DropPickRequest> GetRequestsByDriverId(int driverId)
        {
            return _context.DropPickRequests.Where(r => r.DriverId == driverId).ToList();
        }

        public void AddRequest(DropPickRequest request)
        {
            _context.DropPickRequests.Add(request);
            _context.SaveChanges();
        }

        public void UpdateRequest(DropPickRequest request)
        {
            _context.DropPickRequests.Update(request);
            _context.SaveChanges();
        }

        public void DeleteRequest(int requestId)
        {
            var request = _context.DropPickRequests.FirstOrDefault(r => r.RequestId == requestId);

            if (request != null)
            {
                _context.DropPickRequests.Remove(request);
                _context.SaveChanges();
            }
        }
    }
}