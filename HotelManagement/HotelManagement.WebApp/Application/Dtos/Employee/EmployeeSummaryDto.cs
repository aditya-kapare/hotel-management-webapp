using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.Employee
{
    public sealed class EmployeeSummaryDto
    {
        public string AadharNo { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string MobileNo { get; init; } = string.Empty;
        public string? EmployeePosition { get; init; }
        public Gender Gender { get; init; }
    }
}