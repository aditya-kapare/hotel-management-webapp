using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels
{
    public sealed class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}