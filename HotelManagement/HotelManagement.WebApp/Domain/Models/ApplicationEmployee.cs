using Microsoft.AspNetCore.Identity;
using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    // Identity-backed Employee
    public class ApplicationEmployee : IdentityUser
    {
        // Your original key becomes a Required + Unique field (configured in DbContext).
        [Required]
        [MaxLength(12)]
        public string AadharNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public Gender Gender { get; set; }

        [MaxLength(100)]
        public string? EmployeePosition { get; set; }

        // "xxx.00" formatting is UI; DB stores decimal(18,2) (configured in DbContext).
        public decimal Salary { get; set; }

        // Your original MobileNo maps to IdentityUser.PhoneNumber.
        // To keep your existing contract/field name in code, expose a wrapper property.
        [Required]
        [MaxLength(20)]
        public string MobileNo
        {
            get => PhoneNumber ?? string.Empty;
            set => PhoneNumber = value;
        }

        // Your original EmailId maps to IdentityUser.Email (wrapper to keep name).
        [MaxLength(256)]
        public string? EmailId
        {
            get => Email;
            set => Email = value;
        }
    }
}
