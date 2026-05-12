using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Room
{
    public sealed class RoomDto
    {
        public int RoomNo { get; init; }
        public RoomType RoomType { get; init; }
        public AcOption AcOption { get; init; }
        public AvailabilityStatus AvailabilityStatus { get; init; }
        public CleanStatus CleanStatus { get; init; }
        public decimal Price { get; init; }
    }
}

