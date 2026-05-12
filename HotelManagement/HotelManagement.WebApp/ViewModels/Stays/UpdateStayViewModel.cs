using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Stays
{
    public class UpdateStayViewModel
    {
        [Required]
        public int StayId { get; set; }

        [Required]
        [Display(Name = "Customer Identity ID")]
        public string CustomerIdentityId { get; set; } = string.Empty;

        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int RoomNo { get; set; }

        [Required]
        [Display(Name = "Check-In Date & Time")]
        public DateTime CheckInAt { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Pending Amount")]
        public decimal PendingAmount { get; set; }
    }
}