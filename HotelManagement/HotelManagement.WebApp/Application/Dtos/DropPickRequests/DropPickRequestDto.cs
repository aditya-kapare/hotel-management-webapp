using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.DropPickRequests
{
    public sealed class DropPickRequestDto
    {
        public int RequestId { get; init; }

        public string Notes { get; init; } = string.Empty;
        public RequestType RequestType { get; init; }

        public DropPickStatus Status { get; init; }

        public int StayId { get; init; }
        public int DriverId { get; init; }
        public string DriverName { get; init; } = string.Empty;
        public string CustomerName { get; init; } = string.Empty;
        public string CustomerPhone { get; init; } = string.Empty;
        public int RoomNo { get; init; }
        public bool CanEdit { get; init; }
    }
}
