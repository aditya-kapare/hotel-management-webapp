using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    public class DropPickRequest
    {
        [Key]
        [Required]
        public int RequestId { get; set; }
        [Required]
        public DateTime RequestedAt { get; set; }
        public string Notes { get; set; } = string.Empty;
        [Required]
        public RequestType RequestType { get; set; }
        [Required]
        public int StayId { get; set; }
        public Stay Stay { get; set; } = null!;
        [Required]
        public int DriverId { get; set; }
        public CabDriver CabDriver { get; set; } = null!;

        public DropPickStatus Status { get; set; }
    }
}
