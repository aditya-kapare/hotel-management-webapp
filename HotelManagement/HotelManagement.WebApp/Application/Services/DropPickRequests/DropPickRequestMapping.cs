using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.DropPickRequests
{
    internal static class DropPickRequestMapping
    {
        internal static DropPickRequestDto ToDto(DropPickRequest r) => new()
        {
            RequestId = r.RequestId,
            RequestedAt = r.RequestedAt,
            Notes = r.Notes,
            RequestType = r.RequestType,
            StayId = r.StayId,
            DriverId = r.DriverId,
            RequestStatus = r.Status,
        };

        internal static DropPickRequestDto ToDto(DropPickRequest r, string driver, string customer) => new()
        {
            RequestId = r.RequestId,
            RequestedAt = r.RequestedAt,
            Notes = r.Notes,
            RequestType = r.RequestType,
            StayId = r.StayId,
            DriverId = r.DriverId,
            DriverName = driver,
            CustomerName = customer
        };

        internal static DropPickRequest ToEntity(CreateDropPickRequest r, DateTime requestedAt) => new()
        {
            RequestedAt = requestedAt,
            Notes = r.Notes,
            RequestType = r.RequestType,
            StayId = r.StayId,
            DriverId = r.DriverId
        };

        internal static void Apply(UpdateDropPickRequest r, DropPickRequest entity, DateTime requestedAt)
        {
            entity.RequestedAt = requestedAt;
            entity.Notes = r.Notes;
            entity.RequestType = r.RequestType;
            entity.DriverId = r.DriverId;
            entity.Status = r.Status;
        }
        internal static void Apply(UpdateDropPickRequest normalized, DropPickRequest existing)
        {

            existing.RequestedAt = normalized.RequestedAt;
            existing.RequestType = normalized.RequestType;
            existing.DriverId = normalized.DriverId;
            existing.Notes = normalized.Notes;
            existing.Status = normalized.Status;

        }

        internal static DropPickRequestDto MapToDto(
            DropPickRequest r,
            Dictionary<int, Stay> stayMap,
            Dictionary<string, Customer> customerMap,
            Dictionary<int, CabDriver> driverMap)
            {
                stayMap.TryGetValue(r.StayId, out var stay);

                Customer? customer = null;
                if (stay != null)
                    customerMap.TryGetValue(stay.CustomerIdentityId, out customer);

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
            }
    }
}