using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.DropPickRequests
{
    public sealed class DropPickRequestDto
    {
        public int RequestId { get; init; }
        public DateTime RequestedAt { get; init; }
        public string Notes { get; init; } = string.Empty;
        public RequestType RequestType { get; init; }

        public int StayId { get; init; }
        public int DriverId { get; init; }

        // ✅ Enriched read-only fields for UI
        public string CustomerName { get; init; } = string.Empty;
        public int RoomNo { get; init; }
        public string DriverName { get; init; } = string.Empty;
    }
}
