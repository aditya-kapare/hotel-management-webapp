using System.ComponentModel.DataAnnotations;
using HotelManagement.WebApp.Domain.Enums;

namespace HotelManagement.WebApp.ViewModels.Customers
{
    public sealed class CreateCustomerViewModel
    {
        [Required]
        [Display(Name = "Identity ID")]
        public string IdentityId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Identity Type")]
        public IdentityIdType IdentityIdType { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
    }
}