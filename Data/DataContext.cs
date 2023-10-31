using Microsoft.EntityFrameworkCore;
using ParkingGarageManagement.Models;

namespace ParkingGarageManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingLot> ParkingLots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LicensePlateId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.VehicleType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TicketType)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ParkingLot>(entity =>
            {
                entity.Property(e => e.OccupiedBy)
                    .HasColumnType("TEXT");
            });
        }
    }
}

