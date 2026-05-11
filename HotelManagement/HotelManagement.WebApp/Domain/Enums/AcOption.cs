using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Enums
{
    public enum AcOption
    {
        [Display(Name = "AC")]
        AC,

        [Display(Name = "Non-AC")]
        NonAC
    }
}
