using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public string IdentityId { get; set; } = string.Empty;

        [Required]
        public IdentityIdType IdentityIdType { get; set; } = IdentityIdType.Aadhar;

        [Required]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public ICollection<Stay> Stays { get; set; } = [];
    }
}
