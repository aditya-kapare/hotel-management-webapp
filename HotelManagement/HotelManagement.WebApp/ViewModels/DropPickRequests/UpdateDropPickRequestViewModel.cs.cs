using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.DropPickRequests
{
    public sealed class UpdateDropPickRequestViewModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Request Type")]
        public RequestType RequestType { get; set; }

        [Required]
        [Display(Name = "Status")]
        public DropPickStatus Status { get; set; }

        [Display(Name = "Requested At")]
        public DateTime RequestedAt { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        public string CurrentDriverName { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}