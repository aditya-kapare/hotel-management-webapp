using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Services.Stays
{
    public sealed class StayChargeCalculator : IStayChargeCalculator
    {
        public int CalculateNights(DateTime checkInAt, DateTime checkOutAt)
        {
            if (checkOutAt <= checkInAt) return 1;

            var totalDays = (checkOutAt - checkInAt).TotalDays;
            var nights = (int)Math.Ceiling(totalDays);
            return Math.Max(1, nights);
        }

        public decimal CalculateTotalCharge(decimal roomPricePerNight, int nights) => roomPricePerNight * nights;

        public (decimal TotalCharge, decimal TotalPaid, decimal Pending) CalculateBill(
            decimal roomPricePerNight,
            DateTime checkInAt,
            DateTime checkOutAt,
            decimal depositPaid,
            decimal additionalAmountPaid)
        {
            var nights = CalculateNights(checkInAt, checkOutAt);
            var total = CalculateTotalCharge(roomPricePerNight, nights);

            var totalPaid = depositPaid + additionalAmountPaid;
            var pending = Math.Max(0, total - totalPaid);

            return (total, totalPaid, pending);
        }
    }
}
