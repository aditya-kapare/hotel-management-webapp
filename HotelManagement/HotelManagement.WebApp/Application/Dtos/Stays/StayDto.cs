namespace HotelManagement.WebApp.Application.Dtos.Stays
{
    public sealed class StayDto
    {
        public int StayId { get; init; }

        public int RoomNo { get; init; }
        public DateTime CheckInAt { get; init; }
        public DateTime? CheckOutAt { get; init; }

        public decimal DepositPaid { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal PendingAmount { get; init; }

        public string CustomerIdentityId { get; init; } = string.Empty;

        public string CustomerName { get; init; } = string.Empty;
        public string MobileNo { get; init; } = string.Empty;
    }
}