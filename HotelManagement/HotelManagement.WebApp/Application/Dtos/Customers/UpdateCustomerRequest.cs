using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Customers
{
    public sealed class UpdateCustomerRequest
    {
        public string MobileNo { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        public Gender Gender { get; init; }

        public string Address { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
    }
}