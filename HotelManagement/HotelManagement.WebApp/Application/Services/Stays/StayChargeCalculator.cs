using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Services.Stays
{
    /// <summary>
    /// Calculates stay duration and billing amounts.
    /// </summary>
    public sealed class StayChargeCalculator : IStayChargeCalculator
    {
        /// <summary>
        /// Calculates the number of chargeable nights for a stay.
        /// </summary>
        public int CalculateNights(DateTime checkInAt, DateTime checkOutAt)
        {
            // Ensure minimum one-night charge
            if (checkOutAt <= checkInAt) return 1;

            var totalDays = (checkOutAt - checkInAt).TotalDays;
            var nights = (int)Math.Ceiling(totalDays);
            return Math.Max(1, nights);
        }

        /// <summary>
        /// Calculates total room charge based on nights stayed.
        /// </summary>
        public decimal CalculateTotalCharge(decimal roomPricePerNight, int nights)
            => roomPricePerNight * nights;

        /// <summary>
        /// Calculates total bill, paid amount, and pending balance.
        /// </summary>
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