 using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    public class CabDriver
    {
        [Key]
        [Required]
        public int DriverId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string CarVendor { get; set; } = string.Empty;

        public string CarType { get; set; } = string.Empty;

        public ICollection<DropPickRequest> DropPickRequests { get; set; } = new List<DropPickRequest>();
    }
}
