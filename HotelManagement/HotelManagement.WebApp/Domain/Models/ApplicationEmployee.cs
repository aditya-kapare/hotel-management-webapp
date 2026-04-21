using Microsoft.AspNetCore.Identity;
using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    // Identity-backed Employee
    public class ApplicationEmployee : IdentityUser
    {

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


        public decimal Salary { get; set; }

        [Required]
        [MaxLength(20)]
        public string MobileNo
        {
            get => PhoneNumber ?? string.Empty;
            set => PhoneNumber = value;
        }

        
        [MaxLength(256)]
        public string? EmailId
        {
            get => Email;
            set => Email = value;
        }
    }
}
