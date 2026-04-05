using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Drivers
{
    public sealed class CreateCabDriverRequest
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
        public Gender Gender { get; init; }
        public string CarVendor { get; init; } = string.Empty;
        public string CarType { get; init; } = string.Empty;
    }
}