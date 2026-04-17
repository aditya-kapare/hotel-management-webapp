using Azure.Core;
using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.DropPickRequests;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagement.WebApp.ViewModels.DropPickRequests;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class DropPickRequestService : IDropPickRequestService
    {
        private readonly IDropPickRequestDAL _requestDal;
        private readonly IStayDAL _stayDal;
        
        private readonly ICustomerDAL _customerDal;
        private readonly ICabDriverDAL _driverDal;

        public DropPickRequestService(
            IDropPickRequestDAL requestDal,
            IStayDAL stayDal,
            ICustomerDAL customerDal,
            ICabDriverDAL driverDal)
        {
            _requestDal = requestDal;
            _stayDal = stayDal;
            _customerDal = customerDal;
            _driverDal = driverDal;
        }

        public async Task<IReadOnlyList<DropPickRequestDto>> GetAllAsync()
        {
            var requests = await _requestDal.GetAllRequestsAsync();
            return requests.Select(DropPickRequestMapping.ToDto).ToList();
        }


        public async Task<IReadOnlyList<DropPickRequestDto>> GetRequestListAsync()
        {
            var requests = await _requestDal.GetAllRequestsAsync();
            var stays = await _stayDal.GetAllStaysAsync();
            var customers = await _customerDal.GetAllCustomersAsync();
            var drivers = await _driverDal.GetAllDriversAsync();

            var stayMap = stays.ToDictionary(s => s.StayId);
            var customerMap = customers.ToDictionary(c => c.IdentityId);
            var driverMap = drivers.ToDictionary(d => d.DriverId);

            return requests.Select(r =>
            {
                stayMap.TryGetValue(r.StayId, out var stay);

                Customer? customer = null;
                if (stay != null) 
                
                {
                    customerMap.TryGetValue(stay.CustomerIdentityId, out customer);
                }

                driverMap.TryGetValue(r.DriverId, out var driver);

                return new DropPickRequestDto
                {
                    RequestId = r.RequestId,
                    
                    RequestedAt = r.RequestedAt,
                    Notes = r.Notes,
                    RequestType = r.RequestType,
                    
                    StayId = r.StayId,
                    DriverId = r.DriverId,

                    RoomNo = stay?.RoomNo ?? 0,
                    
                    DriverName = driver?.Name ?? "Unassigned",

                    CustomerName = customer?.Name ?? "Unknown",
                    CustomerPhone = customer?.MobileNo ?? string.Empty,

                    CanEdit = (r.Status == DropPickStatus.Assigned)
                };
            }).ToList();
        }

        public async Task<DropPickRequestDto?> GetByIdAsync(int requestId)
        {
            if (requestId <= 0) return null;
            var req = await _requestDal.GetRequestByIdAsync(requestId);
            return req is null ? null : DropPickRequestMapping.ToDto(req);
        }

        public async Task<IReadOnlyList<DropPickRequestDto>> GetByStayIdAsync(int stayId)
        {
            if (stayId <= 0) return [];
            var requests = await _requestDal.GetRequestsByStayIdAsync(stayId);
            return requests.Select(DropPickRequestMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<DropPickRequestDto>> GetByDriverIdAsync(int driverId)
        {
            if (driverId <= 0) return [];
            var requests = await _requestDal.GetRequestsByDriverIdAsync(driverId);
            return requests.Select(DropPickRequestMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<CabDriver>> GetAvailableDriversAsync()
        {
            return (await _requestDal.GetAvailableDriversAsync()).ToList();
        }

        public async Task<DropPickRequestDto> CreateAsync(CreateDropPickRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);

            var stay = await _stayDal.GetStayByIdAsync(normalized.StayId);
            if (stay is null)
                throw new KeyNotFoundException($"Stay '{normalized.StayId}' was not found.");
            if (stay.CheckOutAt is not null)
                throw new InvalidOperationException($"Stay '{normalized.StayId}' is already checked out.");

            var availableDrivers = await _requestDal.GetAvailableDriversAsync();
            if (!availableDrivers.Any(d => d.DriverId == normalized.DriverId))
                throw new InvalidOperationException("Selected driver is not available.");

            var entity = new DropPickRequest
            {
                StayId = normalized.StayId,
                DriverId = normalized.DriverId,
                RequestType = normalized.RequestType,
                Notes = normalized.Notes,
                RequestedAt = normalized.RequestedAt ?? DateTime.Now,
                Status = DropPickStatus.Assigned
            };

            var created = await _requestDal.AddRequestAsync(entity);
            if (!created)
                throw new InvalidOperationException("Failed to create drop/pick request.");

            return DropPickRequestMapping.ToDto(entity);
        }

        public async Task<DropPickRequestDto> UpdateAsync(int requestId, UpdateDropPickRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (requestId <= 0) throw new ArgumentException("RequestId must be positive.", nameof(requestId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var existing = await _requestDal.GetRequestByIdAsync(requestId);
            if (existing is null)
                throw new KeyNotFoundException($"Request '{requestId}' was not found.");

            var stay = await _stayDal.GetStayByIdAsync(existing.StayId);
            if (stay is null)
                throw new KeyNotFoundException($"Stay '{existing.StayId}' was not found.");
            if (stay.CheckOutAt is not null)
                throw new InvalidOperationException($"Stay '{existing.StayId}' is already checked out.");

            if (normalized.DriverId != existing.DriverId)
            {
                var availableDrivers = await _requestDal.GetAvailableDriversAsync();
                if (!availableDrivers.Any(d => d.DriverId == normalized.DriverId))
                    throw new InvalidOperationException("Selected driver is not available.");
            }

            DropPickRequestMapping.Apply(normalized, existing);
            var updated = await _requestDal.UpdateRequestAsync(existing);
            if (!updated)
                throw new InvalidOperationException($"Failed to update request '{requestId}'.");

            return DropPickRequestMapping.ToDto(existing);
        }

        public async Task<bool> DeleteAsync(int requestId)
        {
            if (requestId <= 0) return false;
            return await _requestDal.DeleteRequestAsync(requestId);
        }

        private static CreateDropPickRequest NormalizeCreate(CreateDropPickRequest r) => new()
        {
            RequestedAt = r.RequestedAt,
            Notes = (r.Notes ?? string.Empty).Trim(),
            RequestType = r.RequestType,
            StayId = r.StayId,
            DriverId = r.DriverId
        };

        private static UpdateDropPickRequest NormalizeUpdate(UpdateDropPickRequest r) => new()
        {
            RequestedAt = r.RequestedAt,
            Notes = (r.Notes ?? string.Empty).Trim(),
            RequestType = r.RequestType,
            DriverId = r.DriverId
        };

        private static void ValidateCreate(CreateDropPickRequest r) 
        {
            if (r.StayId <= 0) throw new ArgumentException("StayId must be positive.", nameof(r.StayId));
            if (r.DriverId <= 0) throw new ArgumentException("DriverId must be positive.", nameof(r.DriverId));
        }

        private static void ValidateUpdate(UpdateDropPickRequest r)
        {
            if (r.DriverId <= 0) throw new ArgumentException("DriverId must be positive.", nameof(r.DriverId));
        }
    }
}