using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Enums
{
    public enum CleanStatus
    {
        [Display(Name = "Clean")]
        Clean,

        [Display(Name = "Dirty")]
        Dirty
    }
}
