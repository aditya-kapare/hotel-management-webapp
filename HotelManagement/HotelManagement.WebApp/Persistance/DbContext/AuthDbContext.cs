using HotelManagement.WebApp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DbContext
{
    /// <summary>
    /// Authentication database context for managing employee identity data.
    /// </summary>
    public class AuthDbContext : IdentityDbContext<ApplicationEmployee>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply base identity configurations
            base.OnModelCreating(builder);

            builder.Entity<ApplicationEmployee>(e =>
            {
                // Enforce unique Aadhaar number
                e.HasIndex(x => x.AadharNo).IsUnique();

                // Define salary precision
                e.Property(x => x.Salary).HasColumnType("decimal(18,2)");

                // Mark required employee fields
                e.Property(x => x.AadharNo).IsRequired();
                e.Property(x => x.Name).IsRequired();
            });
        }
    }
}