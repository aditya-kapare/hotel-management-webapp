using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Rooms
{
    internal static class RoomMapping
    {
        internal static RoomDto ToDto(Room r) => new()
        {
            RoomNo = r.RoomNo,
            RoomType = r.RoomType,
            AcOption = r.AcOption,
            AvailabilityStatus = r.AvailabilityStatus,
            CleanStatus = r.CleanStatus,
            Price = r.Price
        };

        internal static Room ToEntity(CreateRoomRequest req) => new()
        {
            RoomNo = req.RoomNo,
            RoomType = req.RoomType,
            AcOption = req.AcOption,
            AvailabilityStatus = req.AvailabilityStatus,
            CleanStatus = req.CleanStatus,
            Price = req.Price
        };

        internal static void Apply(UpdateRoomRequest req, Room r)
        {
            r.RoomType = req.RoomType;
            r.AcOption = req.AcOption;
            r.AvailabilityStatus = req.AvailabilityStatus;
            r.CleanStatus = req.CleanStatus;
            r.Price = req.Price;
        }
    }
}