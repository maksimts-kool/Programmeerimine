using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Tund10.Avalonia.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Tund10.Avalonia;

public partial class MainWindow : Window
{
    private readonly AutoDbContext _db = new();

    public MainWindow()
    {
        InitializeComponent();
        _db.Database.EnsureCreated();

        LoadOwners();
        LoadCars();
        LoadServices();
        LoadCarServices();
    }

    // -------------------- LOAD METHODS --------------------

    private void LoadOwners()
    {
        var owners = _db.Owners
            .Select(o => new { o.Id, o.FullName, o.Phone })
            .ToList();

        OwnersGrid.ItemsSource = owners;
        OwnerCombo.ItemsSource = _db.Owners.ToList();
        OwnerCombo.SelectedItem = null;
    }

    private void LoadCars()
    {
        var cars = _db.Cars
            .Include(c => c.Owner)
            .Select(c => new
            {
                c.Id,
                c.Brand,
                c.Model,
                c.RegistrationNumber,
                c.OwnerId,
                OwnerName = c.Owner.FullName
            })
            .ToList();

        CarsGrid.ItemsSource = cars;
        LogCarCombo.ItemsSource = _db.Cars.ToList();
    }

    private void LoadServices()
    {
        var services = _db.Services
            .Select(s => new { s.Id, s.Name, s.Price })
            .ToList();

        ServicesGrid.ItemsSource = services;
        LogServiceCombo.ItemsSource = _db.Services.ToList();
    }

    private void LoadCarServices()
    {
        var logs = _db.CarServices
            .Include(cs => cs.Car)
            .Include(cs => cs.Service)
            .Select(cs => new
            {
                cs.CarId,
                cs.ServiceId,
                cs.Mileage,
                cs.DateOfService,
                Car = cs.Car.RegistrationNumber,
                Service = cs.Service.Name
            })
            .ToList();

        LogsGrid.ItemsSource = logs;
    }

    // -------------------- OWNERS --------------------

    private void AddOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(OwnerNameBox.Text) ||
            string.IsNullOrWhiteSpace(OwnerPhoneBox.Text))
        {
            ShowAlert("Please fill in both Full Name and Phone.");
            return;
        }

        var exists = _db.Owners.Any(o =>
            o.FullName.ToLower() == OwnerNameBox.Text.Trim().ToLower() &&
            o.Phone.Trim() == OwnerPhoneBox.Text.Trim());

        if (exists)
        {
            ShowAlert("This owner already exists.");
            return;
        }

        _db.Owners.Add(new Owner
        {
            FullName = OwnerNameBox.Text.Trim(),
            Phone = OwnerPhoneBox.Text.Trim()
        });
        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert("Owner added successfully!");
        OwnerNameBox.Clear();
        OwnerPhoneBox.Clear();
    }

    private void UpdateOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (OwnersGrid.SelectedItem is null)
        {
            ShowAlert("Select an owner to update.");
            return;
        }

        dynamic selected = OwnersGrid.SelectedItem;
        int id = selected.Id;
        var owner = _db.Owners.Find(id);
        if (owner is null) return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(OwnerNameBox.Text) &&
            OwnerNameBox.Text.Trim() != owner.FullName)
        {
            owner.FullName = OwnerNameBox.Text.Trim();
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(OwnerPhoneBox.Text) &&
            OwnerPhoneBox.Text.Trim() != owner.Phone)
        {
            owner.Phone = OwnerPhoneBox.Text.Trim();
            changed = true;
        }

        if (!changed)
        {
            ShowAlert("Nothing to update.");
            return;
        }

        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert("Owner updated successfully!");
    }

    private void DeleteOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (OwnersGrid.SelectedItem is null)
        {
            ShowAlert("Select an owner to delete.");
            return;
        }

        dynamic selected = OwnersGrid.SelectedItem;
        int id = selected.Id;

        var owner = _db.Owners.Include(o => o.Cars).FirstOrDefault(o => o.Id == id);
        if (owner == null) return;

        if (owner.Cars.Any())
        {
            ShowAlert("Cannot delete this owner. Remove related cars first.");
            return;
        }

        _db.Owners.Remove(owner);
        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert("Owner deleted successfully.");
    }

    // -------------------- CARS --------------------

    private void AddCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CarBrandBox.Text) ||
            string.IsNullOrWhiteSpace(CarModelBox.Text) ||
            string.IsNullOrWhiteSpace(CarRegBox.Text) ||
            OwnerCombo.SelectedItem is not Owner selectedOwner)
        {
            ShowAlert("Please fill in all car fields and choose an owner.");
            return;
        }

        var reg = CarRegBox.Text.Trim().ToUpper();

        var exists = _db.Cars.Any(c => c.RegistrationNumber.ToUpper() == reg);
        if (exists)
        {
            ShowAlert("A car with this registration already exists.");
            return;
        }

        _db.Cars.Add(new Car
        {
            Brand = CarBrandBox.Text.Trim(),
            Model = CarModelBox.Text.Trim(),
            RegistrationNumber = reg,
            OwnerId = selectedOwner.Id
        });

        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert("Car added successfully!");
        CarBrandBox.Clear();
        CarModelBox.Clear();
        CarRegBox.Clear();
    }

    private void UpdateCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (CarsGrid.SelectedItem is null)
        {
            ShowAlert("Select a car to update.");
            return;
        }

        dynamic selected = CarsGrid.SelectedItem;
        int id = selected.Id;

        var car = _db.Cars.Find(id);
        if (car == null) return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(CarBrandBox.Text) &&
            CarBrandBox.Text.Trim() != car.Brand)
        {
            car.Brand = CarBrandBox.Text.Trim();
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(CarModelBox.Text) &&
            CarModelBox.Text.Trim() != car.Model)
        {
            car.Model = CarModelBox.Text.Trim();
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(CarRegBox.Text) &&
            CarRegBox.Text.Trim().ToUpper() != car.RegistrationNumber.ToUpper())
        {
            car.RegistrationNumber = CarRegBox.Text.Trim().ToUpper();
            changed = true;
        }

        if (OwnerCombo.SelectedItem is Owner newOwner &&
            newOwner.Id != car.OwnerId)
        {
            car.OwnerId = newOwner.Id;
            changed = true;
        }

        if (!changed)
        {
            ShowAlert("Nothing to update.");
            return;
        }

        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert("Car updated successfully!");
    }

    private void DeleteCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (CarsGrid.SelectedItem is null)
        {
            ShowAlert("Select a car to delete.");
            return;
        }

        dynamic selected = CarsGrid.SelectedItem;
        int id = selected.Id;

        var car = _db.Cars.Include(c => c.CarServices)
            .FirstOrDefault(c => c.Id == id);

        if (car == null) return;

        if (car.CarServices.Any())
        {
            ShowAlert("Cannot delete this car. Remove service logs first.");
            return;
        }

        _db.Cars.Remove(car);
        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert("Car deleted successfully!");
    }

    // -------------------- SERVICES --------------------

    private void AddServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceNameBox.Text) ||
            string.IsNullOrWhiteSpace(ServicePriceBox.Text))
        {
            ShowAlert("Please fill in both Service Name and Price.");
            return;
        }

        if (!decimal.TryParse(ServicePriceBox.Text, out decimal price) || price <= 0)
        {
            ShowAlert("Please enter a valid positive price.");
            return;
        }

        string name = ServiceNameBox.Text.Trim().ToLower();
        if (_db.Services.Any(s => s.Name.ToLower() == name))
        {
            ShowAlert("This service already exists.");
            return;
        }

        _db.Services.Add(new Service { Name = ServiceNameBox.Text.Trim(), Price = price });
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Service added successfully!");
        ServiceNameBox.Clear();
        ServicePriceBox.Clear();
    }

    private void UpdateServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ServicesGrid.SelectedItem is null)
        {
            ShowAlert("Select a service to update.");
            return;
        }

        dynamic selected = ServicesGrid.SelectedItem;
        int id = selected.Id;
        var service = _db.Services.Find(id);
        if (service == null) return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(ServiceNameBox.Text) &&
            ServiceNameBox.Text.Trim() != service.Name)
        {
            service.Name = ServiceNameBox.Text.Trim();
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(ServicePriceBox.Text) &&
            decimal.TryParse(ServicePriceBox.Text, out decimal newPrice) &&
            newPrice != service.Price)
        {
            service.Price = newPrice;
            changed = true;
        }

        if (!changed)
        {
            ShowAlert("Nothing to update.");
            return;
        }

        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Service updated successfully!");
    }

    private void DeleteServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ServicesGrid.SelectedItem is null)
        {
            ShowAlert("Select a service to delete.");
            return;
        }

        dynamic selected = ServicesGrid.SelectedItem;
        int id = selected.Id;

        var service = _db.Services.Include(s => s.CarServices)
            .FirstOrDefault(s => s.Id == id);

        if (service == null) return;

        if (service.CarServices.Any())
        {
            ShowAlert("Cannot delete this service. Remove linked car-service logs first.");
            return;
        }

        _db.Services.Remove(service);
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Service deleted successfully!");
    }

    // -------------------- MAINTENANCE LOGS --------------------

    private void AddLogBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (LogCarCombo.SelectedItem is not Car car ||
            LogServiceCombo.SelectedItem is not Service service ||
            !LogDatePicker.SelectedDate.HasValue ||
            string.IsNullOrWhiteSpace(LogMileageBox.Text))
        {
            ShowAlert("Please fill in all maintenance fields (Car, Service, Date, Mileage).");
            return;
        }

        if (!int.TryParse(LogMileageBox.Text, out int mileage) || mileage <= 0)
        {
            ShowAlert("Invalid mileage value.");
            return;
        }

        DateTime date = LogDatePicker.SelectedDate.Value.Date;

        bool exists = _db.CarServices.Any(cs =>
            cs.CarId == car.Id &&
            cs.ServiceId == service.Id &&
            cs.DateOfService.Date == date);

        if (exists)
        {
            ShowAlert("A log for this combination on this date already exists.");
            return;
        }

        _db.CarServices.Add(new CarService
        {
            CarId = car.Id,
            ServiceId = service.Id,
            Mileage = mileage,
            DateOfService = date
        });

        _db.SaveChanges();
        LoadCarServices();
        ShowAlert("Maintenance record added successfully!");
    }

    private void DeleteLogBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (LogsGrid.SelectedItem is null)
        {
            ShowAlert("Select a record to delete.");
            return;
        }

        dynamic selected = LogsGrid.SelectedItem;
        int carId = selected.CarId;
        int svcId = selected.ServiceId;
        DateTime date = selected.DateOfService;

        var log = _db.CarServices.Find(carId, svcId, date);
        if (log == null) return;

        _db.CarServices.Remove(log);
        _db.SaveChanges();
        LoadCarServices();
        ShowAlert("Maintenance record deleted successfully!");
    }

    // -------------------- UTILITY --------------------

    private async void ShowAlert(string message, string title = "Info")
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
        title,
        message,
        ButtonEnum.Ok
    );

        await box.ShowAsync();
    }
}