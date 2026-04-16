namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class DeleteDropPickRequestViewModel
    {
        public int RequestId { get; init; }

        public int RoomNo { get; init; }

        public string DriverName { get; init; } = string.Empty;

        public string CustomerName { get; init; } = string.Empty;

        public string RequestType { get; set; } = string.Empty;
    }
}