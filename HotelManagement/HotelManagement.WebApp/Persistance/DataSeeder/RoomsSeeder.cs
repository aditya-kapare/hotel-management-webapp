using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class RoomsSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            if (await db.Rooms.AnyAsync(cancellationToken))
                return;

            var rooms = new List<Room>
            {
                // ---------- Single Bed (4 rooms) ----------
                new Room { RoomType = RoomType.SingleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1200m },
                new Room { RoomType = RoomType.SingleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1500m },
                new Room { RoomType = RoomType.SingleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1200m },
                new Room { RoomType = RoomType.SingleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1500m },

                // ---------- Double Bed (8 rooms) ----------
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1800m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2200m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2200m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1800m },

                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2300m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1900m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2300m },
                new Room { RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1900m },

                // ---------- Semi Deluxe (5 rooms) ----------
                new Room { RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2800m },
                new Room { RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 3000m },
                new Room { RoomType = RoomType.SemiDeluxe, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2500m },
                new Room { RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 3000m },
                new Room { RoomType = RoomType.SemiDeluxe, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2600m },

                // ---------- Deluxe (3 rooms) ----------
                new Room { RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 4500m },
                new Room { RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 4800m },
                new Room { RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 5000m }
            };

            await db.Rooms.AddRangeAsync(rooms, cancellationToken);
        }
    }
}
