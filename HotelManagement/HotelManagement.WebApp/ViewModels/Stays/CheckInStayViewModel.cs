using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Stays
{
    public sealed class CheckInStayViewModel
    {
        [Required]
        public string CustomerIdentityId { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int RoomNo { get; set; }

        [Required]
        [Display(Name = "Check-In Date & Time")]
        public DateTime CheckInAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Deposit Paid")]
        public decimal DepositPaid { get; set; }
    }
}