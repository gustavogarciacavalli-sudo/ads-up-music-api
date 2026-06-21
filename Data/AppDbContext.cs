using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Models;

namespace BeatFlowApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Playlist> Playlists => Set<Playlist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>()
            .HasOne(a => a.Genre)
            .WithMany(g => g.Artists)
            .HasForeignKey(a => a.GenreId);

        modelBuilder.Entity<Track>()
            .HasOne(t => t.Artist)
            .WithMany(a => a.Tracks)
            .HasForeignKey(t => t.ArtistId);

        modelBuilder.Entity<Track>()
            .HasOne(t => t.Playlist)
            .WithMany(p => p.Tracks)
            .HasForeignKey(t => t.PlaylistId)
            .IsRequired(false);
    }
}
