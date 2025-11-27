using Tund10.Avalonia.Models;
using Microsoft.EntityFrameworkCore;

namespace Tund10.Avalonia;

public class AutoDbContext : DbContext
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<CarService> CarServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
    "Server=(localdb)\\MSSQLLocalDB;Database=AutoAppDb;Integrated Security=True;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<CarService>()
            .HasKey(cs => new { cs.CarId, cs.ServiceId, cs.DateOfService });

        mb.Entity<CarService>()
            .HasOne(cs => cs.Car)
            .WithMany(c => c.CarServices)
            .HasForeignKey(cs => cs.CarId);

        mb.Entity<CarService>()
            .HasOne(cs => cs.Service)
            .WithMany(s => s.CarServices)
            .HasForeignKey(cs => cs.ServiceId);
    }
}