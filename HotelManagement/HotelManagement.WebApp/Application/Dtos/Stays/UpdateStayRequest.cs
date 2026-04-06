namespace HotelManagement.WebApp.Application.Dtos.Stays
{
    public sealed class UpdateStayRequest
    {
        public int RoomNo { get; init; }
        public DateTime CheckInAt { get; init; }

        public decimal DepositPaid { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal PendingAmount { get; init; }
    }
}   