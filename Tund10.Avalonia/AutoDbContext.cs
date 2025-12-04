using Tund10.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System;
using System.IO;

namespace Tund10.Avalonia;

public class AutoDbContext : DbContext
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<CarService> CarServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            options.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=AutoAppDb;Integrated Security=True;");
        }
        else
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "AutoAppDb_mac.db");
            options.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
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