using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.DAL
{
    /// <summary>
    /// Data access layer for managing drop and pick requests.
    /// </summary>
    public class DropPickRequestDAL : IDropPickRequestDAL
    {
        private readonly HotelDbContext _context;


        public DropPickRequestDAL(HotelDbContext context)
        {
            
            _context = context;
        }

        /// <summary>
        /// Retrieves all drop-pick requests with related data.
        /// </summary>
        //URL [HttpGet] - /api/droppickrequests
        public async Task<IEnumerable<DropPickRequest>> GetAllRequestsAsync()
        {
            // Load requests with driver and customer details
            return await _context.DropPickRequests
                                    .Include(r => r.CabDriver)
                                    .Include(r => r.Stay)
                                        .ThenInclude(s => s.Customer)
                                    .AsNoTracking()
                                    .ToListAsync();
        }
        //URL [HttpGet] - /api/droppickrequests//{requestId:int}/droppickrequest
        public async Task<DropPickRequest?> GetRequestByIdAsync(int requestId)
        {
       
            return await _context.DropPickRequests
                .Include(r => r.Stay)
                    .ThenInclude(s => s.Customer)
                .Include(r => r.CabDriver)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
        }


         //URL [HttpGet] - /api/droppickrequests/{stayId:int}/droppickrequest
        public async Task<IEnumerable<DropPickRequest>> GetRequestsByStayIdAsync(int stayId)
        {
            // Filter requests by stay ID
            return await _context.DropPickRequests
                                .Where(r => r.StayId == stayId)
                                .Include(r => r.Stay)
                                    .ThenInclude(s => s.Customer)
                                .Include(r => r.CabDriver)
                                .ToListAsync();
        }

        //URL [HttpGet] - /api/droppickrequests/{driverId:int}/droppickrequest
        public async Task<IEnumerable<DropPickRequest>> GetRequestsByDriverIdAsync(int driverId)
        {
            // Filter requests by driver ID
            return await _context.DropPickRequests
                                .Where(r => r.DriverId == driverId)
                                .Include(r => r.Stay)
                                    .ThenInclude(s => s.Customer)
                                .Include(r => r.CabDriver)
                                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all available cab drivers.
        /// </summary>
        //URL [HttpGet] - /api/cabdrivers?availability=available
        //URL [HttpGet] - /api/cabdrivers/available
        public async Task<IEnumerable<CabDriver>> GetAvailableDriversAsync()
        {
            // Identify drivers currently assigned to active requests
            var busyDriverIds = await _context.DropPickRequests
                .Where(r => r.Status != DropPickStatus.Completed && r.Status != DropPickStatus.Cancelled)
                .Select(r => r.DriverId)
                .Distinct()
                .ToListAsync();

            return await _context.CabDrivers
                .Where(d => !busyDriverIds.Contains(d.DriverId))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new drop-pick request.
        /// </summary>
        //URL [HttpPost] - /api/droppickrequests

        public async Task<bool> AddRequestAsync(DropPickRequest request)
        {
            // Assign default status if not set
            if (request.Status == default)
                request.Status = DropPickStatus.Assigned;

            // Check if the selected driver is already busy
            bool isBusy = await _context.DropPickRequests.AnyAsync(r =>
                r.DriverId == request.DriverId &&
                (r.Status != DropPickStatus.Completed &&
                r.Status != DropPickStatus.Cancelled));

            if (isBusy) return false;

            await _context.DropPickRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }

        //URL [HttpPut] - /api/droppickrequests
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
                // Handle concurrent update failure
                return false;
            }
        }


        //URL [HttpDelete] - /api/droppickrequests/{requestId:int}
        public async Task<bool> DeleteRequestAsync(int requestId)
        {
     
            var request = await _context.DropPickRequests
                .Where(r => r.RequestId == requestId)
                .ExecuteDeleteAsync();

            return request > 0;
        }
    }
}