// CabDriverDTO (read)
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Drivers
{
    public record CabDriverDTO(
        int DriverId,
        string GovernmentId,
        string Name,
        int Age,
        int Gender,
        string CarVendor,
        string CarType
    );
}