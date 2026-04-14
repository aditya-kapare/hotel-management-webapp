using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Stays
{
    public sealed class CheckInStayViewModel
    {
        public string? SearchIdentityId { get; set; }
        [Required]
        public string CustomerIdentityId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Room Number")]
        public int RoomNo { get; set; }

        [Required]
        [Display(Name = "Check-In Date & Time")]
        public DateTime? CheckInAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Deposit Paid")]
        public decimal DepositPaid { get; set; }

        public bool CustomerFound { get; set; }
    }
}
