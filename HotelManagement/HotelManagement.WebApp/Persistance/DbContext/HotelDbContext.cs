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
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Employee>()
                        .HasKey(e => e.AadharNo);

        
        }
    }
}
