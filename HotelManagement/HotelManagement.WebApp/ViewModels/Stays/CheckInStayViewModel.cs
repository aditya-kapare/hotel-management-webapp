using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.ViewModels.Stays
{
    public sealed class CheckInStayViewModel
    {
        [Required]
        public string CustomerIdentityId { get; set; } = string.Empty;

        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }

        // ✅ New fields
        [Required(ErrorMessage = "Room type is required")]
        public string? RoomType { get; set; }

        [Required(ErrorMessage = "AC option is required")]
        public string? AcOption { get; set; }

        [Required(ErrorMessage = "Room number is required")]
        public int? RoomNo { get; set; }

        public decimal Price { get; set; }

        public string ActionType { get; set; } = "Filter";

        [Required]
        public DateTime CheckInAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Deposit is required")]
        public decimal DepositPaid { get; set; }

        // ✅ Dropdown data (server‑side)
        public IEnumerable<SelectListItem> RoomTypes { get; set; } = [];
        public IEnumerable<SelectListItem> AcOptions { get; set; } = [];
        public List<SelectListItem> AvailableRooms { get; set; } = new();



    }
}