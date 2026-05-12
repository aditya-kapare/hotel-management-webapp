using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.Application.Dtos.DropPickRequests
{
    /// <summary>
    /// DTO used for updating an existing Drop / Pick request.
    /// Contains both editable fields and minimal contextual data
    /// required by the application layer.
    /// </summary>
    public sealed class UpdateDropPickRequest
    {
        // =========================
        // Identity
        // =========================
        public int RequestId { get; init; }

        // =========================
        // Editable fields
        // =========================
        public DateTime RequestedAt { get; init; }

        public RequestType RequestType { get; init; }

        public DropPickStatus Status { get; init; }

        public int DriverId { get; init; }

        public string Notes { get; init; } = string.Empty;

        // =========================
        // Contextual (read-only)
        // =========================
        public string DriverName { get; init; } = string.Empty;
    }
}