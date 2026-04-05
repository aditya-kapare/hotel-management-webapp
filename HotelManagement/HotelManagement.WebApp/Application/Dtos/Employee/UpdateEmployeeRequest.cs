using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Employee
{
    public sealed class UpdateEmployeeRequest
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
        public Gender Gender { get; init; }
        public string? EmployeePosition { get; init; }
        public decimal Salary { get; init; }
        public string MobileNo { get; init; } = string.Empty;
        public string? EmailId { get; init; }
    }
}