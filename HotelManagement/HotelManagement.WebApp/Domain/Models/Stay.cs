using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApp.Domain.Models
{
    public class Stay
    {
        [Key]
        [Required]
        public int StayId { get; set; }

        [Required]
        public DateTime CheckInAt { get; set; }

        public DateTime? CheckOutAt { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DepositPaid { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PendingAmount { get; set; }

        [Required]
        public int RoomNo { get; set; }

        public Room Room { get; set; } = null!;

        [Required]
        public string CustomerIdentityId { get; set; } = string.Empty;

        public Customer Customer { get; set; }

        public ICollection<DropPickRequest> DropPickRequests { get; set; }
    }
}
