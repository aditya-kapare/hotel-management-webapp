using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Customers
{
    public sealed class EditCustomerViewModel
    {
        [Required]
        public string IdentityId { get; set; } = default!;

        [Required]
        public IdentityIdType IdentityIdType { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = default!;

        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string MobileNo { get; set; } = default!;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Country { get; set; } = default!;

        [Required]
        public string? Address { get; set; }
    }
}