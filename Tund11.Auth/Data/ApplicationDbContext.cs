using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tund11.Models;

namespace Tund11.Auth.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Pyha> Pyhad => Set<Pyha>();
    public DbSet<Kylaline> Kylalised => Set<Kylaline>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Kylaline>()
            .HasOne(k => k.Pyha)
            .WithMany(p => p.Kylalised)
            .HasForeignKey(k => k.PyhaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
