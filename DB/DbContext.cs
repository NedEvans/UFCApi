using Microsoft.EntityFrameworkCore;
using AzureAPI.Objects;

namespace AzureAPI.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Fighter> Fighters { get; set; } = null!;
        public DbSet<Fight> Fights { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fighter enums
            modelBuilder.Entity<Fighter>()
                .Property(f => f.WeightClass)
                .HasConversion<byte>();

            modelBuilder.Entity<Fighter>()
                .Property(f => f.Gender)
                .HasConversion<byte>();

            // Fight enums
            modelBuilder.Entity<Fight>()
                .Property(f => f.WeightClass)
                .HasConversion<byte>();

            modelBuilder.Entity<Fight>()
                .Property(f => f.MethodOfVictory)
                .HasConversion<byte>();

            // Fight binary arrays – enforce fixed length
            modelBuilder.Entity<Fight>()
                .Property(f => f.RedHeadStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedBodyStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedLegStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedDistanceStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedClinchStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedGroundStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedTakedowns)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedSubAttempts)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.RedKnockdowns)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueHeadStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueBodyStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueLegStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueDistanceStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueClinchStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueGroundStrikes)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueTakedowns)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueSubAttempts)
                .HasMaxLength(5)
                .IsFixedLength();

            modelBuilder.Entity<Fight>()
                .Property(f => f.BlueKnockdowns)
                .HasMaxLength(5)
                .IsFixedLength();
        }
    }
}
