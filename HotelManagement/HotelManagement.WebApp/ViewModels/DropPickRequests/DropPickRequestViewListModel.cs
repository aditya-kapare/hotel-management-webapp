namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class DropPickRequestViewListModel
    {
        public int RequestId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;

        public int RoomNo { get; set; }

        public string DriverName { get; set; } = string.Empty;

        public string RequestType { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; }

        public bool CanEdit { get; set; }
    }
}