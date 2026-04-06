namespace HotelManagement.WebApp.Application.Interfaces.Services
{
    public interface IStayChargeCalculator
    {
        int CalculateNights(DateTime checkInAt, DateTime checkOutAt);
        decimal CalculateTotalCharge(decimal roomPricePerNight, int nights);
        (decimal TotalCharge, decimal TotalPaid, decimal Pending) CalculateBill(
            decimal roomPricePerNight,
            DateTime checkInAt,
            DateTime checkOutAt,
            decimal depositPaid,
            decimal additionalAmountPaid);
    }
}
