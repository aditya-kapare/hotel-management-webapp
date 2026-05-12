namespace HotelManagement.WebApp.Application.Dtos.Room
{
    public record RoomDTO(
        int RoomNo,
        int RoomType,
        int AcOption,
        int AvailabilityStatus,
        int CleanStatus,
        decimal Price
    );
}
    