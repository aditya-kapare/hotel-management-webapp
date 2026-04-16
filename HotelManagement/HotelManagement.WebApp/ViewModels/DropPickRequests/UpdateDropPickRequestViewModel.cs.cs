using System;
using System.ComponentModel.DataAnnotations;
using HotelManagement.WebApp.Domain.Enums;

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

        public string Notes { get; set; } = string.Empty;
    }
}