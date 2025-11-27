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
        string projectDir = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..")
        );

        string dbPath = Path.Combine(projectDir, "TootedDB.mdf");

        string connectionString =
            $@"Data Source=(LocalDB)\MSSQLLocalDB;
           AttachDbFilename={dbPath};
           Integrated Security=True;
           Initial Catalog=TootedDB_File;
           Connect Timeout=30;";

        optionsBuilder.UseSqlServer(connectionString);
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
