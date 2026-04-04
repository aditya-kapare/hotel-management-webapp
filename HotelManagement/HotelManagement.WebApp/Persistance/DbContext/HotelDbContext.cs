using HotelManagement.WebApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Stay> Stays { get; set; }
        public DbSet<CabDriver> CabDrivers { get; set; }
        public DbSet<DropPickRequest> DropPickRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee: string PK
            modelBuilder.Entity<Employee>()
                        .HasKey(e => e.AadharNo);

            // Room: PK by convention if RoomNo is int and named RoomNo
            modelBuilder.Entity<Room>()
                        .HasKey(r => r.RoomNo);

            // Customer: string PK (IdentityId)
            modelBuilder.Entity<Customer>()
                        .HasKey(c => c.IdentityId);

            // Stay relationships:
            // Stay -> Room (many stays can point to a room over time)
            modelBuilder.Entity<Stay>()
                        .HasOne(s => s.Room)
                        .WithMany(r => r.Stays)
                        .HasForeignKey(s => s.RoomNo)
                        .OnDelete(DeleteBehavior.Restrict);

            // Stay -> Customer (many stays for one customer over time)
            modelBuilder.Entity<Stay>()
                        .HasOne(s => s.Customer)
                        .WithMany(c => c.Stays)
                        .HasForeignKey(s => s.CustomerIdentityId)
                        .OnDelete(DeleteBehavior.Restrict);

            // DropPickRequest relationships:
            modelBuilder.Entity<DropPickRequest>()
                        .HasOne(d => d.Stay)
                        .WithMany(s => s.DropPickRequests)
                        .HasForeignKey(d => d.StayId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DropPickRequest>()
                        .HasOne(d => d.CabDriver)
                        .WithMany(cd => cd.DropPickRequests)
                        .HasForeignKey(d => d.DriverId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Optional but very useful: decimal precision for money
            modelBuilder.Entity<Employee>()
                        .Property(e => e.Salary)
                        .HasPrecision(18, 2);

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