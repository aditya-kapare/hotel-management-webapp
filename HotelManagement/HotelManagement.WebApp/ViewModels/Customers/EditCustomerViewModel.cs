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
        public string Name { get; set; } = default!;

        [Required]
        public string MobileNo { get; set; } = default!;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string Country { get; set; } = default!;

        public string? Address { get; set; }
    }
}