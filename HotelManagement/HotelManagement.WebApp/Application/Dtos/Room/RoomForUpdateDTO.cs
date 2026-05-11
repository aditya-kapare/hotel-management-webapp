namespace HotelManagement.WebApp.Application.Dtos.Room
{
    public record RoomForUpdateDTO(
        int RoomType,
        int AcOption,
        int AvailabilityStatus,
        int CleanStatus,
        decimal Price
    );
}