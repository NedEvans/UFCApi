using Microsoft.EntityFrameworkCore;
using UFCApi.CSVObjects;

namespace UFCApi.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<EventCsv> EventsCsv { get; set; } = null!;
        public DbSet<FighterCsv> FightersCsv { get; set; } = null!;
        public DbSet<FightCsv> FightsCsv { get; set; } = null!;
        public DbSet<RoundCsv> RoundsCsv { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoundCsv>()
                .HasKey(r => new { r.FightId, r.FighterId, r.Round });

            modelBuilder.Entity<FightCsv>()
                .HasOne(f => f.Fighter1)
                .WithMany()
                .HasForeignKey(f => f.Fighter1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FightCsv>()
                .HasOne(f => f.Fighter2)
                .WithMany()
                .HasForeignKey(f => f.Fighter2Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FightCsv>()
                .HasOne(f => f.Winner)
                .WithMany()
                .HasForeignKey(f => f.WinnerId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);

        }
    }
}
