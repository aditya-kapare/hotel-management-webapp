using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class DropPickRequestsSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            if (await db.DropPickRequests.AnyAsync(cancellationToken))
                return;

            var stays = await db.Stays
                .OrderBy(s => s.StayId)
                .Take(6)
                .ToListAsync(cancellationToken);

            var drivers = await db.CabDrivers
                .OrderBy(d => d.DriverId)
                .Take(4)
                .ToListAsync(cancellationToken);

            if (stays.Count < 4 || drivers.Count < 2)
                return;

            var now = DateTime.Now;

            var requests = new List<DropPickRequest>
            {
                // ---------------- PICK requests ----------------
                new DropPickRequest
                {
                    RequestedAt = now.AddDays(-2),
                    RequestType = RequestType.Pick,
                    Notes = "Airport pickup requested",
                    StayId = stays[0].StayId,
                    DriverId = drivers[0].DriverId
                },
                new DropPickRequest
                {
                    RequestedAt = now.AddDays(-1),
                    RequestType = RequestType.Pick,
                    Notes = "Railway station pickup",
                    StayId = stays[1].StayId,
                    DriverId = drivers[1].DriverId
                },

                // ---------------- DROP requests ----------------
                new DropPickRequest
                {
                    RequestedAt = now.AddHours(-6),
                    RequestType = RequestType.Drop,
                    Notes = "Drop to airport after checkout",
                    StayId = stays[2].StayId,
                    DriverId = drivers[2].DriverId
                },
                new DropPickRequest
                {
                    RequestedAt = now.AddHours(-3),
                    RequestType = RequestType.Drop,
                    Notes = "Drop to city center",
                    StayId = stays[3].StayId,
                    DriverId = drivers[3].DriverId
                },

                // ---------------- Mixed / additional ----------------
                new DropPickRequest
                {
                    RequestedAt = now.AddDays(-1).AddHours(-2),
                    RequestType = RequestType.Pick,
                    Notes = "Late night pickup",
                    StayId = stays[4].StayId,
                    DriverId = drivers[0].DriverId
                },
                new DropPickRequest
                {
                    RequestedAt = now.AddHours(-1),
                    RequestType = RequestType.Drop,
                    Notes = "Early morning drop",
                    StayId = stays[5].StayId,
                    DriverId = drivers[1].DriverId
                }
            };

            await db.DropPickRequests.AddRangeAsync(requests, cancellationToken);
        }
    }
}