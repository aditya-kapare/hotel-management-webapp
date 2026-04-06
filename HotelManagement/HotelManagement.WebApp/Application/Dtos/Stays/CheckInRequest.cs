namespace HotelManagement.WebApp.Application.Dtos.Stays
{
    public sealed class CheckInRequest
    {
        public string CustomerIdentityId { get; init; } = string.Empty;
        public int RoomNo { get; init; }

        public DateTime? CheckInAt { get; init; }
        public decimal DepositPaid { get; init; }
    }
}