namespace HotelManagement.WebApp.Application.Dtos.Room
{
    public record RoomForCreationDTO(
        int RoomNo,
        int RoomType,
        int AcOption,
        decimal Price
    );
}
