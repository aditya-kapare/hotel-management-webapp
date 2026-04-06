namespace HotelManagement.WebApp.Application.Dtos.Stays
{
    public sealed class CheckOutRequest
    {
        public DateTime? CheckOutAt { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal PendingAmount { get; init; }
    }
}