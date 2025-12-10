using System;
using System.Linq;
using Tund10.Avalonia.Models;

namespace Tund10.Avalonia;

public static class SeedData
{
    public static void Seed(AutoDbContext db)
    {
        if (db.Owners.Any()) return;

        // Add default roles
        if (!db.Roles.Any())
        {
            db.Roles.AddRange(
                new Role { Name = "Owner", CanViewOwners = true, CanManageOwners = true, CanViewCars = true, CanManageCars = true, CanViewServices = true, CanManageServices = true, CanChangeStatus = true, CanViewWorkers = true, CanManageWorkers = true },
                new Role { Name = "Manager", CanViewOwners = true, CanManageOwners = true, CanViewCars = true, CanManageCars = true, CanViewServices = true, CanManageServices = true, CanChangeStatus = true, CanViewWorkers = false, CanManageWorkers = false },
                new Role { Name = "Mechanic", CanViewOwners = false, CanManageOwners = false, CanViewCars = true, CanManageCars = false, CanViewServices = true, CanManageServices = true, CanChangeStatus = true, CanViewWorkers = false, CanManageWorkers = false },
                new Role { Name = "Viewer", CanViewOwners = true, CanManageOwners = false, CanViewCars = true, CanManageCars = false, CanViewServices = true, CanManageServices = false, CanChangeStatus = false, CanViewWorkers = false, CanManageWorkers = false }
            );
            db.SaveChanges();
        }

        // Add default workers
        if (!db.Workers.Any(w => w.Name == "admin"))
        {
            db.Workers.Add(new Worker
            {
                Name = "admin",
                Password = "admin",
                Role = "Owner"
            });
            db.SaveChanges();
        }

        // Add 100 Owners
        var firstNames = new[] { "Jaan", "Mari", "Peeter", "Kati", "Toomas", "Liis", "Andres", "Kristiina", "Mart", "Anna", "Mati", "Kadri", "Priit", "Tiina", "Rein", "Ene", "Jaak", "Piret", "Ants", "Marika" };
        var lastNames = new[] { "Tamm", "Sepp", "Saar", "Mägi", "Kask", "Kukk", "Rebane", "Ilves", "Karu", "Lepp", "Pärn", "Raud", "Kivi", "Jõgi", "Mets", "Nurk", "Org", "Soo", "Laas", "Teder" };
        var owners = new Owner[100];
        for (int i = 0; i < 100; i++)
        {
            owners[i] = new Owner
            {
                FullName = $"{firstNames[i % firstNames.Length]} {lastNames[(i / firstNames.Length) % lastNames.Length]}",
                Phone = $"+372 5{i:D3} {(i * 11) % 10000:D4}"
            };
        }
        db.Owners.AddRange(owners);
        db.SaveChanges();

        // Add 100 Services
        var serviceNames = new[] { "Õlivahetus", "Rehvivahetus", "Pidurite kontroll", "Kliimaseadme täitmine", "Diagnostika", "Hooldus", "Remont", "Pesu", "Poleerimine", "Keevitus" };
        var services = new Service[100];
        for (int i = 0; i < 100; i++)
        {
            services[i] = new Service
            {
                Name = $"{serviceNames[i % serviceNames.Length]} {i / serviceNames.Length + 1}",
                Price = decimal.Round(50 + (i * 10) % 500, 2)
            };
        }
        db.Services.AddRange(services);
        db.SaveChanges();

        // Add 100 Cars
        var brands = new[] { "Toyota", "BMW", "Mercedes", "Audi", "Volkswagen", "Ford", "Opel", "Nissan", "Honda", "Mazda" };
        var models = new[] { "Sedan", "Wagon", "SUV", "Coupe", "Hatchback" };
        var cars = new Car[100];
        for (int i = 0; i < 100; i++)
        {
            cars[i] = new Car
            {
                Brand = brands[i % brands.Length],
                Model = models[i % models.Length],
                RegistrationNumber = $"{(char)('A' + i % 26)}{(char)('A' + (i / 26) % 26)}{(char)('A' + (i / 676) % 26)}{i:D3}",
                OwnerId = owners[i].Id
            };
        }
        db.Cars.AddRange(cars);
        db.SaveChanges();

        // Add 100 CarServices
        var random = new Random(42);
        var carServices = new CarService[100];
        for (int i = 0; i < 100; i++)
        {
            carServices[i] = new CarService
            {
                CarId = cars[i].Id,
                ServiceId = services[random.Next(100)].Id,
                DateOfService = DateTime.Now.AddDays(-random.Next(365)).Date.AddHours(random.Next(24)),
                Mileage = 10000 + random.Next(200000)
            };
        }
        db.CarServices.AddRange(carServices);
        db.SaveChanges();
    }
}
