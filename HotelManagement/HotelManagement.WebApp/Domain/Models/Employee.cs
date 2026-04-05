using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public string AadharNo { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string? EmployeePosition { get; set; }

        public decimal Salary { get; set; }

        [Required]
        public string MobileNo { get; set; } = string.Empty;

        public string? EmailId { get; set; }
    }
}