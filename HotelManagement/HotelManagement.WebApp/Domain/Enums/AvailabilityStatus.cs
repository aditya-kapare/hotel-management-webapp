using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Enums
{
    public enum AvailabilityStatus
    {
        [Display(Name = "Available")]
        Available,
        [Display(Name = "Occupied")]
        Occupied
    }
}
