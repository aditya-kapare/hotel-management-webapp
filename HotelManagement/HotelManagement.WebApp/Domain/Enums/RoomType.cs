using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Enums
{
    public enum RoomType
    {
        [Display(Name = "Single Bed")]
        SingleBed,

        [Display(Name = "Double Bed")]
        DoubleBed,

        [Display(Name = "Semi Deluxe")]
        SemiDeluxe,
        [Display(Name = "Deluxe")]
        Deluxe
    }
}
