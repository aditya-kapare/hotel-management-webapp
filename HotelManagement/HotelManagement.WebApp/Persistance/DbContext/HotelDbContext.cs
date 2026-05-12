using HotelManagement.WebApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data
{
    /// <summary>
    /// Main database context for hotel management data.
    /// </summary>
    public class HotelDbContext : DbContext
    {

        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Stay> Stays { get; set; }
        public DbSet<CabDriver> CabDrivers { get; set; }
        public DbSet<DropPickRequest> DropPickRequests { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply base EF Core configurations
            base.OnModelCreating(modelBuilder);

            // Configure primary keys
            modelBuilder.Entity<Room>()
                        .HasKey(r => r.RoomNo);

            modelBuilder.Entity<Customer>()
                        .HasKey(c => c.IdentityId);

            // Configure stay to room relationship
            modelBuilder.Entity<Stay>()
                        .HasOne(s => s.Room)
                        .WithMany(r => r.Stays)
                        .HasForeignKey(s => s.RoomNo)
                        .OnDelete(DeleteBehavior.Restrict);

            // Configure stay to customer relationship
            modelBuilder.Entity<Stay>()
                        .HasOne(s => s.Customer)
                        .WithMany(c => c.Stays)
                        .HasForeignKey(s => s.CustomerIdentityId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Configure drop-pick request to stay relationship
            modelBuilder.Entity<DropPickRequest>()
                        .HasOne(d => d.Stay)
                        .WithMany(s => s.DropPickRequests)
                        .HasForeignKey(d => d.StayId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Configure drop-pick request to cab driver relationship
            modelBuilder.Entity<DropPickRequest>()
                        .HasOne(d => d.CabDriver)
                        .WithMany(cd => cd.DropPickRequests)
                        .HasForeignKey(d => d.DriverId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Configure monetary field precision
            modelBuilder.Entity<Room>()
                        .Property(r => r.Price)
                        .HasPrecision(18, 2);

            modelBuilder.Entity<Stay>()
                        .Property(s => s.DepositPaid)
                        .HasPrecision(18, 2);

            modelBuilder.Entity<Stay>()
                        .Property(s => s.AmountPaid)
                        .HasPrecision(18, 2);

            modelBuilder.Entity<Stay>()
                        .Property(s => s.PendingAmount)
                        .HasPrecision(18, 2);
        }
    }
}