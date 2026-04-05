using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DataSeeder
{
    public class StaysSeeder : ISeeder
    {
        public async Task SeedAsync(HotelDbContext db, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("StaysSeeder started");
            if (await db.Stays.AnyAsync(cancellationToken))
                return;

            var rooms = await db.Rooms
                .OrderBy(r => r.RoomNo)
                .Take(10)
                .ToListAsync(cancellationToken);

            var customers = await db.Customers
                .OrderBy(c => c.IdentityId)
                .Take(8)
                .ToListAsync(cancellationToken);

            if (rooms.Count < 10 || customers.Count < 4)
                return; 

            var now = DateTime.Now;


            var stays = new List<Stay>
            {
                // ---------------- Completed stays (CheckOutAt is NOT null) ----------------
                new Stay
                {
                    CheckInAt = now.AddDays(-20).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-18).Date.AddHours(10),
                    DepositPaid = 1000m,
                    AmountPaid = 3600m,
                    PendingAmount = 0m,
                    RoomNo = rooms[0].RoomNo,
                    CustomerIdentityId = customers[0].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-16).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-14).Date.AddHours(10),
                    DepositPaid = 1000m,
                    AmountPaid = 4400m,
                    PendingAmount = 0m,
                    RoomNo = rooms[1].RoomNo,
                    CustomerIdentityId = customers[1].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-12).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-11).Date.AddHours(10),
                    DepositPaid = 800m,
                    AmountPaid = 1800m,
                    PendingAmount = 0m,
                    RoomNo = rooms[2].RoomNo,
                    CustomerIdentityId = customers[2].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-10).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-8).Date.AddHours(10),
                    DepositPaid = 1200m,
                    AmountPaid = 5200m,
                    PendingAmount = 0m,
                    RoomNo = rooms[3].RoomNo,
                    CustomerIdentityId = customers[3].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-7).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-6).Date.AddHours(10),
                    DepositPaid = 1000m,
                    AmountPaid = 3000m,
                    PendingAmount = 0m,
                    RoomNo = rooms[4].RoomNo,
                    CustomerIdentityId = customers[4].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-5).Date.AddHours(12),
                    CheckOutAt = now.AddDays(-3).Date.AddHours(10),
                    DepositPaid = 1500m,
                    AmountPaid = 9000m,
                    PendingAmount = 0m,
                    RoomNo = rooms[5].RoomNo,
                    CustomerIdentityId = customers[5].IdentityId
                },

                // ---------------- Active stays (CheckOutAt IS null) ----------------
                new Stay
                {
                    CheckInAt = now.AddDays(-2).Date.AddHours(13),
                    CheckOutAt = null,
                    DepositPaid = 1000m,
                    AmountPaid = 1000m,
                    PendingAmount = 2500m,
                    RoomNo = rooms[6].RoomNo,
                    CustomerIdentityId = customers[6].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-1).Date.AddHours(13),
                    CheckOutAt = null,
                    DepositPaid = 1200m,
                    AmountPaid = 1200m,
                    PendingAmount = 3200m,
                    RoomNo = rooms[7].RoomNo,
                    CustomerIdentityId = customers[7].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-3).Date.AddHours(13),
                    CheckOutAt = null,
                    DepositPaid = 800m,
                    AmountPaid = 800m,
                    PendingAmount = 1800m,
                    RoomNo = rooms[8].RoomNo,
                    CustomerIdentityId = customers[0].IdentityId
                },
                new Stay
                {
                    CheckInAt = now.AddDays(-4).Date.AddHours(13),
                    CheckOutAt = null,
                    DepositPaid = 1500m,
                    AmountPaid = 1500m,
                    PendingAmount = 4500m,
                    RoomNo = rooms[9].RoomNo,
                    CustomerIdentityId = customers[1].IdentityId
                }
            };

            // Update Room availability based on stays
            rooms[6].AvailabilityStatus = AvailabilityStatus.Occupied;
            rooms[7].AvailabilityStatus = AvailabilityStatus.Occupied;
            rooms[8].AvailabilityStatus = AvailabilityStatus.Occupied;
            rooms[9].AvailabilityStatus = AvailabilityStatus.Occupied;

            Console.WriteLine($"Seeding stays. Rooms={rooms.Count}, Customers={customers.Count}");

            await db.Stays.AddRangeAsync(stays, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}