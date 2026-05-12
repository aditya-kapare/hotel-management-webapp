// CabDriverForCreationDTO
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Drivers
{
    public record CabDriverForCreationDTO(
        string GovernmentId,
        string Name,
        int Age,
        Gender Gender,
        string CarVendor,
        string CarType
    );
}