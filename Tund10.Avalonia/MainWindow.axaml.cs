using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Tund10.Avalonia.Models;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.Generic;
using Avalonia.Input;

namespace Tund10.Avalonia;

public partial class MainWindow : Window
{
    private readonly AutoDbContext _db = new();
    private SearchResultsWindow? SearchPopup;

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
            ShowAlert("Palun täitke nii täisnimi kui ka telefoninumber.");
            return;
        }

        var exists = _db.Owners.Any(o =>
            o.FullName.ToLower() == OwnerNameBox.Text.Trim().ToLower() &&
            o.Phone.Trim() == OwnerPhoneBox.Text.Trim());

        if (exists)
        {
            ShowAlert("See omanik on juba olemas.");
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
        ShowAlert("Omanik lisati edukalt!");
        OwnerNameBox.Clear();
        OwnerPhoneBox.Clear();
    }

    private void UpdateOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (OwnersGrid.SelectedItem is null)
        {
            ShowAlert("Valige omanik, keda soovite uuendada.");
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
            ShowAlert("Uuendada pole midagi.");
            return;
        }

        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert("Omanik on edukalt uuendatud!");
    }

    private void DeleteOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (OwnersGrid.SelectedItem is null)
        {
            ShowAlert("Valige kustutatav omanik.");
            return;
        }

        dynamic selected = OwnersGrid.SelectedItem;
        int id = selected.Id;

        var owner = _db.Owners.Include(o => o.Cars).FirstOrDefault(o => o.Id == id);
        if (owner == null) return;

        if (owner.Cars.Any())
        {
            ShowAlert("Seda omanikku ei saa kustutada. Eemalda esmalt seotud autod.");
            return;
        }

        _db.Owners.Remove(owner);
        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert("Omanik on edukalt kustutatud.");
    }

    // -------------------- CARS --------------------

    private void AddCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CarBrandBox.Text) ||
            string.IsNullOrWhiteSpace(CarModelBox.Text) ||
            string.IsNullOrWhiteSpace(CarRegBox.Text) ||
            OwnerCombo.SelectedItem is not Owner selectedOwner)
        {
            ShowAlert("Palun täitke kõik auto väljad ja valige omanik.");
            return;
        }

        var reg = CarRegBox.Text.Trim().ToUpper();

        var exists = _db.Cars.Any(c => c.RegistrationNumber.ToUpper() == reg);
        if (exists)
        {
            ShowAlert("Sellise registreerimisnumbriga auto on juba olemas.");
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
        ShowAlert("Auto lisati edukalt!");
        CarBrandBox.Clear();
        CarModelBox.Clear();
        CarRegBox.Clear();
    }

    private void UpdateCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (CarsGrid.SelectedItem is null)
        {
            ShowAlert("Valige uuendatav auto.");
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
            ShowAlert("Uuendada pole midagi.");
            return;
        }

        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert("Auto uuendamine õnnestus!");
    }

    private void DeleteCarBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (CarsGrid.SelectedItem is null)
        {
            ShowAlert("Valige kustutatav auto.");
            return;
        }

        dynamic selected = CarsGrid.SelectedItem;
        int id = selected.Id;

        var car = _db.Cars.Include(c => c.CarServices)
            .FirstOrDefault(c => c.Id == id);

        if (car == null) return;

        if (car.CarServices.Any())
        {
            ShowAlert("Seda autot ei saa kustutada. Kustuta esmalt teeninduslogid.");
            return;
        }

        _db.Cars.Remove(car);
        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert("Auto on edukalt kustutatud!");
    }

    // -------------------- SERVICES --------------------

    private void AddServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceNameBox.Text) ||
            string.IsNullOrWhiteSpace(ServicePriceBox.Text))
        {
            ShowAlert("Palun täitke nii teenuse nimi kui ka hind.");
            return;
        }

        if (!decimal.TryParse(ServicePriceBox.Text, out decimal price) || price <= 0)
        {
            ShowAlert("Palun sisestage kehtiv positiivne hind.");
            return;
        }

        string name = ServiceNameBox.Text.Trim().ToLower();
        if (_db.Services.Any(s => s.Name.ToLower() == name))
        {
            ShowAlert("See teenus on juba olemas.");
            return;
        }

        _db.Services.Add(new Service { Name = ServiceNameBox.Text.Trim(), Price = price });
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Teenus lisati edukalt!");
        ServiceNameBox.Clear();
        ServicePriceBox.Clear();
    }

    private void UpdateServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ServicesGrid.SelectedItem is null)
        {
            ShowAlert("Valige teenus, mida soovite uuendada.");
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
            ShowAlert("Uuendada pole midagi.");
            return;
        }

        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Teenus uuendati edukalt!");
    }

    private void DeleteServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ServicesGrid.SelectedItem is null)
        {
            ShowAlert("Valige teenus, mida soovite kustutada.");
            return;
        }

        dynamic selected = ServicesGrid.SelectedItem;
        int id = selected.Id;

        var service = _db.Services.Include(s => s.CarServices)
            .FirstOrDefault(s => s.Id == id);

        if (service == null) return;

        if (service.CarServices.Any())
        {
            ShowAlert("Seda teenust ei saa kustutada. Eemalda esmalt seotud autoteeninduse logid.");
            return;
        }

        _db.Services.Remove(service);
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert("Teenus kustutati edukalt!");
    }

    // -------------------- MAINTENANCE LOGS --------------------

    private void AddLogBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (LogCarCombo.SelectedItem is not Car car ||
            LogServiceCombo.SelectedItem is not Service service ||
            !LogDatePicker.SelectedDate.HasValue ||
            string.IsNullOrWhiteSpace(LogMileageBox.Text))
        {
            ShowAlert("Palun täitke kõik hooldusväljad.");
            return;
        }

        if (!int.TryParse(LogMileageBox.Text, out int mileage) || mileage <= 0)
        {
            ShowAlert("Lubamatu läbisõidu väärtus.");
            return;
        }

        DateTime date = LogDatePicker.SelectedDate.Value.Date;

        bool exists = _db.CarServices.Any(cs =>
            cs.CarId == car.Id &&
            cs.ServiceId == service.Id &&
            cs.DateOfService.Date == date);

        if (exists)
        {
            ShowAlert("Selle kirje jaoks on sellel kuupäeval juba olemas logi.");
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
        ShowAlert("Hooldusandmed lisati edukalt!");
    }

    private void DeleteLogBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (LogsGrid.SelectedItem is null)
        {
            ShowAlert("Valige kustutatav kirje.");
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
        ShowAlert("Hooldusandmed kustutati edukalt!");
    }

    private void LoginBtn_Click(object? sender, RoutedEventArgs e)
    {
        string username = LoginNameBox.Text?.Trim() ?? "";
        string password = LoginPasswordBox.Text?.Trim() ?? "";

        if (username == "admin" && password == "admin")
        {
            LoginPanel.IsVisible = false;

            OwnerActionPanel.IsVisible = true;
            CarActionPanel.IsVisible = true;
            ServiceActionPanel.IsVisible = true;
            LogActionPanel.IsVisible = true;

            ShowAlert("Tere tulemast, admin!");
        }
        else
        {
            ShowAlert("Vale kasutajanimi või parool!");
        }
    }

    private void SearchBox_KeyUp(object? sender, KeyEventArgs e)
    {
        string query = SearchBox.Text?.Trim().ToLower() ?? "";

        if (query.Length < 3)
        {
            SearchPopup?.Close();
            SearchPopup = null;
            return;
        }

        var results = new List<string>();

        // OWNERS
        foreach (dynamic o in OwnersGrid.ItemsSource!)
        {
            if (((string)o.FullName).ToLower().Contains(query) ||
    ((string)o.Phone).ToLower().Contains(query))
            {
                results.Add($"OMANIKUD • {o.FullName} • {o.Phone}");
                //if (results.Count == 3) break;
            }
        }

        // CARS
        if (results.Count < 3)
        {
            foreach (dynamic c in CarsGrid.ItemsSource!)
            {
                if (((string)c.Brand).ToLower().Contains(query) ||
    ((string)c.Model).ToLower().Contains(query) ||
    ((string)c.RegistrationNumber).ToLower().Contains(query) ||
    ((string)c.OwnerName).ToLower().Contains(query))
                {
                    results.Add($"AUTOD • {c.Brand} {c.Model} ({c.RegistrationNumber})");
                    //if (results.Count == 3) break;
                }
            }
        }

        // SERVICES
        if (results.Count < 3)
        {
            foreach (dynamic s in ServicesGrid.ItemsSource!)
            {
                if (((string)s.Name).ToLower().Contains(query) ||
    s.Price.ToString().Contains(query))
                {
                    results.Add($"TEENUSED • {s.Name}");
                    //if (results.Count == 3) break;
                }
            }
        }

        // LOGS
        if (results.Count < 3 && LogsGrid.ItemsSource is IEnumerable<object> logs)
        {
            foreach (dynamic log in logs)
            {
                if (((string)log.Car).ToLower().Contains(query) ||
                    ((string)log.Service).ToLower().Contains(query) ||
                    log.Mileage.ToString().Contains(query) ||
                    log.DateOfService.ToString("yyyy-MM-dd").Contains(query))
                {
                    // include date for unique identification
                    results.Add($"LOGID • {log.Car} → {log.Service} → {log.DateOfService:yyyy-MM-dd}");
                    if (results.Count == 3) break;
                }
            }
        }

        if (results.Count == 0)
        {
            SearchPopup?.Close();
            SearchPopup = null;
            return;
        }

        if (SearchPopup == null)
        {
            SearchPopup = new SearchResultsWindow(this)
            {
                ShowActivated = false,
            };
        }

        var screenPos = SearchBox.PointToScreen(new Point(0, SearchBox.Bounds.Height));
        SearchPopup.Position = new PixelPoint((int)screenPos.X, (int)screenPos.Y);

        SearchPopup.SetResults(results);
        SearchPopup.Show(this);
    }

    // -------------------- UTILITY --------------------

    private async void ShowAlert(string message)
    {
        var dlg = new Window
        {
            Width = 300,
            Height = 180,
            Title = "Sõnum",
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Topmost = true,
            ShowInTaskbar = false,
            CanResize = false,
        };

        var stack = new StackPanel
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 20,
            Margin = new Thickness(20)
        };

        var text = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var okButton = new Button
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        okButton.Click += (_, _) => dlg.Close();

        stack.Children.Add(text);
        stack.Children.Add(okButton);
        dlg.Content = stack;

        await dlg.ShowDialog(this);
    }
    private void SelectRow(DataGrid grid, Func<dynamic, bool> match)
    {
        if (grid.ItemsSource is IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                dynamic d = item;
                if (match(d))
                {
                    grid.SelectedItem = item;
                    grid.ScrollIntoView(item, null);
                    break;
                }
            }
        }
    }
    public void HandleSearchSelection(string line)
    {
        if (!line.Contains("•")) return;

        string[] parts = line.Split('•');
        string tab = parts[0].Trim().ToUpper();
        string target = parts[1].Trim();

        // OWNERS
        if (tab == "OMANIKUD")
        {
            MainTabs.SelectedIndex = 0;
            SelectRow(OwnersGrid, o => ((string)o.FullName) == target);
        }

        // CARS
        if (tab == "AUTOD")
        {
            MainTabs.SelectedIndex = 1;
            string reg = target.Split('(').Last().Replace(")", "").Trim();
            SelectRow(CarsGrid, c => ((string)c.RegistrationNumber) == reg);
        }

        // SERVICES
        if (tab == "TEENUSED")
        {
            MainTabs.SelectedIndex = 2;
            SelectRow(ServicesGrid, s => ((string)s.Name) == target);
        }

        // LOGS
        if (tab == "LOGID")
        {
            MainTabs.SelectedIndex = 2;

            var parts2 = target.Split('→', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (parts2.Length < 3)
                return;

            string carName = parts2[0];
            string serviceName = parts2[1];
            DateTime date = DateTime.Parse(parts2[2]);

            SelectRow(LogsGrid, l =>
                ((string)l.Car).Equals(carName, StringComparison.OrdinalIgnoreCase) &&
                ((string)l.Service).Equals(serviceName, StringComparison.OrdinalIgnoreCase) &&
                ((DateTime)l.DateOfService).Date == date.Date
            );
        }

        SearchBox.Text = "";
    }
}