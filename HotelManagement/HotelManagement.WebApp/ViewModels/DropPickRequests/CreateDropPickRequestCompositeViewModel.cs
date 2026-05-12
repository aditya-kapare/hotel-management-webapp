using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class CreateDropPickRequestCompositeViewModel
    {
        // ========================
        // CONTEXT (LoadStay / LoadCustomer)
        // ========================
        public int StayId { get; set; }
        public int RoomNo { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobileNo { get; set; } = string.Empty;

        // ========================
        // CREATE INPUTS
        // ========================
        public int DriverId { get; set; }

        public RequestType RequestType { get; set; }

        public DateTime RequestedAt { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<CabDriverBriefDto> Drivers { get; set; } = new();
    }
}
