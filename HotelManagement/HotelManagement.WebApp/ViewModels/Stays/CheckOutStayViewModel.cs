using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Stays
{
    public class CheckOutStayViewModel
    {
        [Required]
        public int StayId { get; set; }

        [Display(Name = "Customer Identity ID")]
        public string CustomerIdentityId { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNo { get; set; }

        [Display(Name = "Check-In Date & Time")]
        public DateTime CheckInAt { get; set; }

        [Required]
        [Display(Name = "Check-Out Date & Time")]
        public DateTime CheckOutAt { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Total Amount Paid")]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Deposit Paid")]
        public decimal DepositPaid { get; set; }
    }
}