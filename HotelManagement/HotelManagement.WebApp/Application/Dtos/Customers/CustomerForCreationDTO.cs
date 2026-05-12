using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Customers
{
    public sealed class CustomerForCreationDto
    {
        public string IdentityId { get; init; } = string.Empty;
        public IdentityIdType IdentityIdType { get; init; }

        public string MobileNo { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        public Gender Gender { get; init; }

        public string Address { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
    }
}