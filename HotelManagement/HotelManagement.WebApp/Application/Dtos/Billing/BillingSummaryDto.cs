namespace HotelManagement.WebApp.Application.Dtos.Billing
{
    public sealed class BillingSummaryDto
    {
        public int Nights { get; init; }
        public decimal RatePerNight { get; init; }

        public decimal TotalCharge { get; init; }

        public decimal DepositPaid { get; init; }
        public decimal AdditionalPaid { get; init; }
        public decimal TotalPaid { get; init; }

        public decimal PendingAmount { get; init; }
    }
}