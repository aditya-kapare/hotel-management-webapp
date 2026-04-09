using HotelManagement.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.WebApp.Domain.Models
{
    public class Room
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoomNo { get; set; }


        [Required]
        public RoomType RoomType { get; set; }

        [Required]
        public AcOption AcOption { get; set; }

        [Required]
        public AvailabilityStatus AvailabilityStatus { get; set; }

        [Required]
        public CleanStatus CleanStatus { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public ICollection<Stay> Stays { get; set; } = [];
    }
}
