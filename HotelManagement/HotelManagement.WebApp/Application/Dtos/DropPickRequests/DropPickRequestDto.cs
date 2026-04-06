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
    }
}