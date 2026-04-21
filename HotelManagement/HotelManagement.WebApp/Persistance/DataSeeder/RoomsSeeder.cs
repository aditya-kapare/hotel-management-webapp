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
            
                new Room {RoomNo=101, RoomType = RoomType.SingleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1200m },
                new Room {RoomNo=102, RoomType = RoomType.SingleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1500m },
                new Room {RoomNo=103, RoomType = RoomType.SingleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1200m },
                new Room {RoomNo=104, RoomType = RoomType.SingleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1500m },
             
          
                new Room {RoomNo=106, RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1800m },
                new Room {RoomNo=107, RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2200m },
                new Room {RoomNo=108, RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2200m },
                new Room {RoomNo=109, RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1800m },

                new Room {RoomNo=111, RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2300m },
                new Room {RoomNo=112, RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1900m },
                new Room {RoomNo=113, RoomType = RoomType.DoubleBed, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2300m },
                new Room {RoomNo=114, RoomType = RoomType.DoubleBed, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 1900m },

                new Room {RoomNo=116, RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2800m },
                new Room {RoomNo=117, RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 3000m },
                new Room {RoomNo=118, RoomType = RoomType.SemiDeluxe, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2500m },
                new Room {RoomNo=119, RoomType = RoomType.SemiDeluxe, AcOption = AcOption.AC,    AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 3000m },
                new Room {RoomNo=120, RoomType = RoomType.SemiDeluxe, AcOption = AcOption.NonAC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 2600m },
                         
             
                new Room {RoomNo=105, RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 4500m },
                new Room {RoomNo=110, RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 4800m },
                new Room {RoomNo=115, RoomType = RoomType.Deluxe, AcOption = AcOption.AC, AvailabilityStatus = AvailabilityStatus.Available, CleanStatus = CleanStatus.Clean, Price = 5000m }
            };            

            await db.Rooms.AddRangeAsync(rooms, cancellationToken);
        }
    }
}
