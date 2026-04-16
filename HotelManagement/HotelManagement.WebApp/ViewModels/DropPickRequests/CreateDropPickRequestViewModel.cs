using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class CreateDropPickRequestViewModel
    {
        [Required]
        public int StayId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public int RoomNo { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Request Type")]
        public RequestType RequestType { get; set; }

        // ✅ UI-only dropdowns
        [Required]
        [Display(Name = "Pick / Drop Time")]
        public TimeSpan SelectedTime { get; set; }

        public string SelectedCarType { get; set; } = string.Empty;
        public string SelectedCarVendor { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}