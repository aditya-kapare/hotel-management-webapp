using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    public class DropPickRequestDAL : IDropPickRequestDAL
    {
        private readonly HotelDbContext _context;

        public DropPickRequestDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DropPickRequest>> GetAllRequestsAsync()
        {
            return await _context.DropPickRequests.ToListAsync();
        }

        public async Task<DropPickRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.DropPickRequests
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
        }

        public async Task<IEnumerable<DropPickRequest>> GetRequestsByStayIdAsync(int stayId)
        {
            return await _context.DropPickRequests
                .Where(r => r.StayId == stayId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DropPickRequest>> GetRequestsByDriverIdAsync(int driverId)
        {
            return await _context.DropPickRequests
                .Where(r => r.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<bool> AddRequestAsync(DropPickRequest request)
        {
            await _context.DropPickRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;        
        }

        public async Task<bool> UpdateRequestAsync(DropPickRequest request)
        {
            _context.DropPickRequests.Update(request);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRequestAsync(int requestId)
        {
            var request = await _context.DropPickRequests
                .Where(r => r.RequestId == requestId)
                .ExecuteDeleteAsync();

            return request > 0;
        }
    }
}