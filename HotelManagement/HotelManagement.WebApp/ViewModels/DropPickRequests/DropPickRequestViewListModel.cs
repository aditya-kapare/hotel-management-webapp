using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class DropPickRequestViewListModel
    {
        public int RequestId { get; set; }
        public DateTime RequestedAt { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public DropPickStatus Status { get; set; }
        public int StayId { get; set; }
        public int RoomNo { get; set; }


        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobileNo { get; set; } = string.Empty;

        public bool CanEdit { get; set; }
    }
}