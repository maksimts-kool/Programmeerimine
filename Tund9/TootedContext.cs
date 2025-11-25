using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Tund9;

public class TootedContext : DbContext
{
    public DbSet<Toode> Tooted { get; set; }
    public DbSet<Kategooria> Kategooriad { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TootedDB;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategooria>().ToTable("Kategooriad");
        modelBuilder.Entity<Toode>().ToTable("Tooted");
    }
    public void EnsureCreated()
    {
        Database.EnsureCreated();
    }
}
