using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tund13.Models;

namespace Tund13.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Keelekursus> Keelekursused { get; set; }
    public DbSet<Opetaja> Opetajad { get; set; }
    public DbSet<Koolitus> Koolitused { get; set; }
    public DbSet<Registreerimine> Registreerimised { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Opetaja → ApplicationUser (optional: one-to-one)
        builder.Entity<Opetaja>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.ApplicationUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // Registreerimine → ApplicationUser
        builder.Entity<Registreerimine>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.ApplicationUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        // Üks õpilane ei saa samale koolitusele kaks korda registreeruda
        builder.Entity<Registreerimine>()
            .HasIndex(r => new { r.KoolitusId, r.ApplicationUserId })
            .IsUnique();
    }
}

