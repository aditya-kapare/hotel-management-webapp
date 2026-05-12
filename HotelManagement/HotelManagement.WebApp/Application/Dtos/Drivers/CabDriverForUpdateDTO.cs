// CabDriverForUpdateDTO
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Drivers
{
    public record CabDriverForUpdateDTO(
        string GovernmentId,
        string Name,
        int Age,
        Gender Gender,
        string CarVendor,
        string CarType
    );
}