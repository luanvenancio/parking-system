using Microsoft.EntityFrameworkCore;
using ParkingSystem.Models;

namespace ParkingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<SpotType> SpotTypes { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeRule> FeeRules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ParkingSession> ParkingSessions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Cars)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserCars"));

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<SpotType>()
                .HasOne(st => st.Fee)
                .WithOne(f => f.SpotType)
                .HasForeignKey<Fee>(f => f.SpotTypeId);
        }
    }
}