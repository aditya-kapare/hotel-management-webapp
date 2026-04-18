namespace HotelManagement.WebApp.Application.Dtos.Stays
{
    public sealed class UpdateStayRequest
    {
        public string CustomerIdentityId { get; init; } = string.Empty;
        public int RoomNo { get; init; }
        public DateTime? CheckInAt { get; init; }

        public decimal DepositPaid { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal PendingAmount { get; init; }
    }
}   