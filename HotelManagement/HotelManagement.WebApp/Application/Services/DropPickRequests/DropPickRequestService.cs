using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.DropPickRequests;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class DropPickRequestService : IDropPickRequestService
    {
        private readonly IDropPickRequestDAL _requestDal;
        private readonly IStayDAL _stayDal;
        private readonly ICabDriverDAL _driverDal;

        public DropPickRequestService(
            IDropPickRequestDAL requestDal,
            IStayDAL stayDal,
            ICabDriverDAL driverDal)
        {
            _requestDal = requestDal;
            _stayDal = stayDal;
            _driverDal = driverDal;
        }

        public async Task<IReadOnlyList<DropPickRequestDto>> GetAllAsync()
        {
            var requests = await _requestDal.GetAllRequestsAsync();
            return requests.Select(DropPickRequestMapping.ToDto).ToList();
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

        public async Task<DropPickRequestDto> CreateAsync(CreateDropPickRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);

            // These reads are justified (business validation)
            var stay = await _stayDal.GetStayByIdAsync(normalized.StayId);
            if (stay is null)
                throw new KeyNotFoundException($"Stay '{normalized.StayId}' was not found.");

            if (stay.CheckOutAt is not null)
                throw new InvalidOperationException($"Stay '{normalized.StayId}' is already checked out.");

            var driver = await _driverDal.GetDriverByIdAsync(normalized.DriverId);
            if (driver is null)
                throw new KeyNotFoundException($"Driver '{normalized.DriverId}' was not found.");

            var requestedAt = normalized.RequestedAt ?? DateTime.Now;

            var entity = DropPickRequestMapping.ToEntity(normalized, requestedAt);
            await _requestDal.AddRequestAsync(entity);

            return DropPickRequestMapping.ToDto(entity);
        }

        public async Task<DropPickRequestDto> UpdateAsync(int requestId, UpdateDropPickRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (requestId <= 0) throw new ArgumentException("RequestId must be positive.", nameof(requestId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            // Required: need StayId from existing request to enforce "no update after checkout"
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
                var driver = await _driverDal.GetDriverByIdAsync(normalized.DriverId);
                if (driver is null)
                    throw new KeyNotFoundException($"Driver '{normalized.DriverId}' was not found.");
            }

            var requestedAt = normalized.RequestedAt == default ? existing.RequestedAt : normalized.RequestedAt;

            DropPickRequestMapping.Apply(normalized, existing, requestedAt);

            var updated = await _requestDal.UpdateRequestAsync(existing);
            if (!updated)
                throw new KeyNotFoundException($"Request '{requestId}' was not found.");

            return DropPickRequestMapping.ToDto(existing);
        }

        public async Task<bool> DeleteAsync(int requestId)
        {
            if (requestId <= 0) return false;

            // ✅ Single DB operation delete (no read-first)
            return await _requestDal.DeleteRequestAsync(requestId);
        }

        // Normalize + validate helpers
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
            if (r.StayId <= 0)
                throw new ArgumentException("StayId must be positive.", nameof(r.StayId));
            if (r.DriverId <= 0)
                throw new ArgumentException("DriverId must be positive.", nameof(r.DriverId));
        }

        private static void ValidateUpdate(UpdateDropPickRequest r)
        {
            if (r.DriverId <= 0)
                throw new ArgumentException("DriverId must be positive.", nameof(r.DriverId));
        }
    }
}