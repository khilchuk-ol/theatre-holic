using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data;

public class TheatreHolicContext : DbContext
{
    public DbSet<Genre>? Genres { get; set; }

    public DbSet<Author>? Authors { get; set; }

    public DbSet<Show>? Shows { get; set; }

    public DbSet<Ticket>? Tickets { get; set; }

    public TheatreHolicContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "server=localhost;user=root;password=root;database=theatreholic;",
            new MySqlServerVersion(new Version(8, 0, 28))
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
        modelBuilder.Entity<Ticket>()
            .HasOne(_ => _.Show)
            .WithMany(_ => _.Tickets)
            .HasForeignKey(_ => _.ShowId);

        modelBuilder.Entity<Show>()
            .HasMany(_ => _.Genres)
            .WithMany("_shows");
    }
}