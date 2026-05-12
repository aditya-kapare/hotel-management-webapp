using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Customers
{
    public sealed class CreateCustomerViewModel
    {
        [Required]
        public string IdentityId { get; set; } = string.Empty;

        [Required]
        public IdentityIdType IdentityIdType { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Country { get; set; } = string.Empty;
    }
}
