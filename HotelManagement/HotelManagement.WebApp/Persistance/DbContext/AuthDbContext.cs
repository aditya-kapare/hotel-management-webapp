using HotelManagement.WebApp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.WebApp.Persistance.DbContext
{
    public class AuthDbContext : IdentityDbContext<ApplicationEmployee>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationEmployee>(e =>
            {
                // Aadhaar unique + indexed for fast lookup (replaces [Key] from old Employee).
                e.HasIndex(x => x.AadharNo).IsUnique();

                // Salary stored as decimal with 2 places.
                e.Property(x => x.Salary).HasColumnType("decimal(18,2)");

                // Optional: enforce required at DB level too
                e.Property(x => x.AadharNo).IsRequired();
                e.Property(x => x.Name).IsRequired();

                // These wrappers write to Identity columns, so no extra mapping needed.
            });
        }
    }
}
