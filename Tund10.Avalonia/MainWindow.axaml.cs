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
using Avalonia.Controls.Templates;
using Avalonia.Data;

namespace Tund10.Avalonia;

public partial class MainWindow : Window
{
    private readonly AutoDbContext _db = new();
    private SearchResultsWindow? SearchPopup;

    public MainWindow()
    {
        InitializeComponent();
        _db.Database.EnsureCreated();
        SeedData.Seed(_db);

        LanguageManager.LoadLanguage();
        LanguageCombo.SelectedIndex = LanguageManager.CurrentLanguage == "RU" ? 1 : 0;
        UpdateLanguage();

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
    }

    private void LoadServices()
    {
        var services = _db.Services
            .Select(s => new { s.Id, s.Name, s.Price })
            .ToList();

        ServicesGrid.ItemsSource = services;
    }

    private void LoadCarServices()
    {
        var logs = _db.CarServices
            .Include(cs => cs.Car)
            .Include(cs => cs.Service)
            .Select(cs => new
            {
                cs.Id,
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

    private async void AddCarForOwner_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int ownerId)
            return;

        var owner = _db.Owners.Find(ownerId);
        if (owner == null) return;

        var addCarWindow = new AddCarWindow(ownerId, owner.FullName, this);
        await addCarWindow.ShowDialog(this);
        LoadCars();
    }

    private void AddOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(OwnerNameBox.Text) ||
            string.IsNullOrWhiteSpace(OwnerPhoneBox.Text))
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        var exists = _db.Owners.Any(o =>
            o.FullName.ToLower() == OwnerNameBox.Text.Trim().ToLower() &&
            o.Phone.Trim() == OwnerPhoneBox.Text.Trim());

        if (exists)
        {
            ShowAlert(LanguageManager.Get("OwnerExists"));
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
        ShowAlert(LanguageManager.Get("OwnerAdded"));
        OwnerNameBox.Clear();
        OwnerPhoneBox.Clear();
    }

    private async void UpdateOwnerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int ownerId)
            return;

        var owner = _db.Owners.Find(ownerId);
        if (owner == null) return;

        var dlg = new Window
        {
            Width = 400,
            Height = 250,
            Title = LanguageManager.Get("UpdateOwner"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        var nameBox = new TextBox { Width = 350, Text = owner.FullName, Watermark = LanguageManager.Get("Name") };
        var phoneBox = new TextBox { Width = 350, Text = owner.Phone, Watermark = LanguageManager.Get("Phone") };

        stack.Children.Add(nameBox);
        stack.Children.Add(phoneBox);

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };

        var saveBtn = new Button { Content = LanguageManager.Get("Save"), Width = 100 };
        saveBtn.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(phoneBox.Text))
            {
                ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            owner.FullName = nameBox.Text.Trim();
            owner.Phone = phoneBox.Text.Trim();
            _db.SaveChanges();
            LoadOwners();
            LoadCars();
            ShowAlert(LanguageManager.Get("OwnerUpdated"));
            dlg.Close();
        };

        var cancelBtn = new Button { Content = LanguageManager.Get("Cancel"), Width = 100 };
        cancelBtn.Click += (_, _) => dlg.Close();

        btnPanel.Children.Add(saveBtn);
        btnPanel.Children.Add(cancelBtn);
        stack.Children.Add(btnPanel);

        dlg.Content = stack;
        await dlg.ShowDialog(this);
    }

    private void DeleteOwnerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int ownerId)
            return;

        var owner = _db.Owners.Include(o => o.Cars).FirstOrDefault(o => o.Id == ownerId);
        if (owner == null) return;

        if (owner.Cars.Any())
        {
            ShowAlert(LanguageManager.Get("CannotDeleteOwner"));
            return;
        }

        _db.Owners.Remove(owner);
        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        ShowAlert(LanguageManager.Get("OwnerDeleted"));
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

    private async void AddServiceForCar_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int carId)
            return;

        var car = _db.Cars.Find(carId);
        if (car == null) return;

        var dlg = new Window
        {
            Width = 400,
            Height = 300,
            Title = LanguageManager.Get("AddServiceTitle"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        stack.Children.Add(new TextBlock
        {
            Text = $"{LanguageManager.Get("Car")}: {car.Brand} {car.Model} ({car.RegistrationNumber})",
            FontWeight = FontWeight.Bold,
            FontSize = 16
        });

        var serviceCombo = new ComboBox
        {
            Width = 350,
            PlaceholderText = LanguageManager.Get("SelectService"),
            ItemsSource = _db.Services.ToList()
        };
        serviceCombo.ItemTemplate = new FuncDataTemplate<Service>((s, _) =>
            new TextBlock { Text = s?.Name });
        stack.Children.Add(serviceCombo);

        var datePicker = new DatePicker { Width = 350, SelectedDate = DateTime.Now, HorizontalAlignment = HorizontalAlignment.Left };
        stack.Children.Add(datePicker);

        var mileageBox = new TextBox { Width = 350, Watermark = LanguageManager.Get("Mileage"), HorizontalAlignment = HorizontalAlignment.Left };
        stack.Children.Add(mileageBox);

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };

        var saveBtn = new Button { Content = LanguageManager.Get("Save"), Width = 100 };
        saveBtn.Click += (_, _) =>
        {
            if (serviceCombo.SelectedItem is not Service service ||
                !datePicker.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(mileageBox.Text))
            {
                ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            if (!int.TryParse(mileageBox.Text, out int mileage) || mileage <= 0)
            {
                ShowAlert(LanguageManager.Get("InvalidMileage"));
                return;
            }

            DateTime date = datePicker.SelectedDate.Value.Date;

            bool exists = _db.CarServices.Any(cs =>
                cs.CarId == carId &&
                cs.ServiceId == service.Id &&
                cs.DateOfService.Date == date);

            if (exists)
            {
                ShowAlert(LanguageManager.Get("LogExists"));
                return;
            }

            _db.CarServices.Add(new CarService
            {
                CarId = carId,
                ServiceId = service.Id,
                Mileage = mileage,
                DateOfService = date
            });

            _db.SaveChanges();
            LoadCarServices();
            ShowAlert(LanguageManager.Get("ServiceAdded"));
            dlg.Close();
        };

        var cancelBtn = new Button { Content = LanguageManager.Get("Cancel"), Width = 100 };
        cancelBtn.Click += (_, _) => dlg.Close();

        btnPanel.Children.Add(saveBtn);
        btnPanel.Children.Add(cancelBtn);
        stack.Children.Add(btnPanel);

        dlg.Content = stack;
        await dlg.ShowDialog(this);
    }



    private async void UpdateCarFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int carId)
            return;

        var car = _db.Cars.Find(carId);
        if (car == null) return;

        var dlg = new Window
        {
            Width = 400,
            Height = 300,
            Title = LanguageManager.Get("UpdateCar"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        var brandBox = new TextBox { Width = 350, Text = car.Brand, Watermark = LanguageManager.Get("Brand") };
        var modelBox = new TextBox { Width = 350, Text = car.Model, Watermark = LanguageManager.Get("Model") };
        var regBox = new TextBox { Width = 350, Text = car.RegistrationNumber, Watermark = LanguageManager.Get("RegNumber") };
        var ownerCombo = new ComboBox { Width = 350, PlaceholderText = LanguageManager.Get("Owner"), ItemsSource = _db.Owners.ToList() };
        ownerCombo.ItemTemplate = new FuncDataTemplate<Owner>((o, _) => new TextBlock { Text = o?.FullName });
        ownerCombo.SelectedItem = _db.Owners.Find(car.OwnerId);

        stack.Children.Add(brandBox);
        stack.Children.Add(modelBox);
        stack.Children.Add(regBox);
        stack.Children.Add(ownerCombo);

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };

        var saveBtn = new Button { Content = LanguageManager.Get("Save"), Width = 100 };
        saveBtn.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(brandBox.Text) ||
                string.IsNullOrWhiteSpace(modelBox.Text) ||
                string.IsNullOrWhiteSpace(regBox.Text) ||
                ownerCombo.SelectedItem is not Owner selectedOwner)
            {
                ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            car.Brand = brandBox.Text.Trim();
            car.Model = modelBox.Text.Trim();
            car.RegistrationNumber = regBox.Text.Trim().ToUpper();
            car.OwnerId = selectedOwner.Id;
            _db.SaveChanges();
            LoadCars();
            LoadCarServices();
            ShowAlert(LanguageManager.Get("CarUpdated"));
            dlg.Close();
        };

        var cancelBtn = new Button { Content = LanguageManager.Get("Cancel"), Width = 100 };
        cancelBtn.Click += (_, _) => dlg.Close();

        btnPanel.Children.Add(saveBtn);
        btnPanel.Children.Add(cancelBtn);
        stack.Children.Add(btnPanel);

        dlg.Content = stack;
        await dlg.ShowDialog(this);
    }

    private void DeleteCarFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int carId)
            return;

        var car = _db.Cars.Include(c => c.CarServices).FirstOrDefault(c => c.Id == carId);
        if (car == null) return;

        if (car.CarServices.Any())
        {
            ShowAlert(LanguageManager.Get("CannotDeleteCar"));
            return;
        }

        _db.Cars.Remove(car);
        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        ShowAlert(LanguageManager.Get("CarDeleted"));
    }



    // -------------------- SERVICES --------------------

    private async void UpdateServiceFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int serviceId)
            return;

        var service = _db.Services.Find(serviceId);
        if (service == null) return;

        var dlg = new Window
        {
            Width = 400,
            Height = 250,
            Title = LanguageManager.Get("UpdateService"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        var nameBox = new TextBox { Width = 350, Text = service.Name, Watermark = LanguageManager.Get("ServiceName") };
        var priceBox = new TextBox { Width = 350, Text = service.Price.ToString(), Watermark = LanguageManager.Get("Price") };

        stack.Children.Add(nameBox);
        stack.Children.Add(priceBox);

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };

        var saveBtn = new Button { Content = LanguageManager.Get("Save"), Width = 100 };
        saveBtn.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(priceBox.Text))
            {
                ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            if (!decimal.TryParse(priceBox.Text, out decimal price) || price <= 0)
            {
                ShowAlert(LanguageManager.Get("InvalidPrice"));
                return;
            }

            service.Name = nameBox.Text.Trim();
            service.Price = price;
            _db.SaveChanges();
            LoadServices();
            LoadCarServices();
            ShowAlert(LanguageManager.Get("ServiceUpdated"));
            dlg.Close();
        };

        var cancelBtn = new Button { Content = LanguageManager.Get("Cancel"), Width = 100 };
        cancelBtn.Click += (_, _) => dlg.Close();

        btnPanel.Children.Add(saveBtn);
        btnPanel.Children.Add(cancelBtn);
        stack.Children.Add(btnPanel);

        dlg.Content = stack;
        await dlg.ShowDialog(this);
    }

    private void DeleteServiceFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int serviceId)
            return;

        var service = _db.Services.Include(s => s.CarServices)
            .FirstOrDefault(s => s.Id == serviceId);

        if (service == null) return;

        if (service.CarServices.Any())
        {
            ShowAlert(LanguageManager.Get("CannotDeleteService"));
            return;
        }

        _db.Services.Remove(service);
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert(LanguageManager.Get("ServiceDeleted"));
    }

    private void AddServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceNameBox.Text) ||
            string.IsNullOrWhiteSpace(ServicePriceBox.Text))
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        if (!decimal.TryParse(ServicePriceBox.Text, out decimal price) || price <= 0)
        {
            ShowAlert(LanguageManager.Get("InvalidPrice"));
            return;
        }

        string name = ServiceNameBox.Text.Trim().ToLower();
        if (_db.Services.Any(s => s.Name.ToLower() == name))
        {
            ShowAlert(LanguageManager.Get("ServiceExists"));
            return;
        }

        _db.Services.Add(new Service { Name = ServiceNameBox.Text.Trim(), Price = price });
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        ShowAlert(LanguageManager.Get("ServiceAdded"));
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

    private void DeleteLogFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int logId)
            return;

        var log = _db.CarServices.Find(logId);
        if (log == null) return;

        _db.CarServices.Remove(log);
        _db.SaveChanges();
        LoadCarServices();
        ShowAlert(LanguageManager.Get("LogDeleted"));
    }

    private void LanguageCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LanguageCombo.SelectedIndex == -1) return;
        LanguageManager.CurrentLanguage = LanguageCombo.SelectedIndex == 1 ? "RU" : "EE";
        UpdateLanguage();
    }

    private void UpdateLanguage()
    {
        Title = LanguageManager.Get("Title");
        LoginBtn.Content = LanguageManager.Get("Login");
        LoginNameBox.Watermark = LanguageManager.Get("Username");
        LoginPasswordBox.Watermark = LanguageManager.Get("Password");
        SearchBox.Watermark = LanguageManager.Get("Search");
        
        ((TabItem)MainTabs.Items[0]!).Header = LanguageManager.Get("Owners");
        ((TabItem)MainTabs.Items[1]!).Header = LanguageManager.Get("Cars");
        ((TabItem)MainTabs.Items[2]!).Header = LanguageManager.Get("ServicesLogs");
        
        OwnerNameBox.Watermark = LanguageManager.Get("Name");
        OwnerPhoneBox.Watermark = LanguageManager.Get("Phone");
        AddOwnerBtn.Content = LanguageManager.Get("Add");
        
        ServiceNameBox.Watermark = LanguageManager.Get("ServiceName");
        ServicePriceBox.Watermark = LanguageManager.Get("Price");
        AddServiceBtn.Content = LanguageManager.Get("Add");
        
        RebuildDataGrids();
        
        LoadOwners();
        LoadCars();
        LoadServices();
        LoadCarServices();
    }

    private void RebuildDataGrids()
    {
        bool isLoggedIn = !LoginPanel.IsVisible;
        
        // Rebuild Owners Grid
        OwnersGrid.Columns.Clear();
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ID"), Binding = new Binding("Id"), Width = new DataGridLength(60) });
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Name"), Binding = new Binding("FullName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Phone"), Binding = new Binding("Phone"), Width = new DataGridLength(150) });
        
        var ownerActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(320), IsVisible = isLoggedIn };
        ownerActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var addCarBtn = new Button { Content = LanguageManager.Get("AddCarBtn"), Classes = { "primary" } };
            addCarBtn.Click += AddCarForOwner_Click;
            addCarBtn.Tag = ((dynamic)item)?.Id;
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateOwnerFromGrid_Click;
            updateBtn.Tag = ((dynamic)item)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteOwnerFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item)?.Id;
            panel.Children.Add(addCarBtn);
            panel.Children.Add(updateBtn);
            panel.Children.Add(deleteBtn);
            return panel;
        });
        OwnersGrid.Columns.Add(ownerActionsCol);
        
        // Rebuild Cars Grid
        CarsGrid.Columns.Clear();
        CarsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ID"), Binding = new Binding("Id"), Width = new DataGridLength(60) });
        CarsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Brand"), Binding = new Binding("Brand"), Width = new DataGridLength(100) });
        CarsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Model"), Binding = new Binding("Model"), Width = new DataGridLength(100) });
        CarsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("RegNumber"), Binding = new Binding("RegistrationNumber"), Width = new DataGridLength(120) });
        CarsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Owner"), Binding = new Binding("OwnerName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        
        var carActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(360), IsVisible = isLoggedIn };
        carActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var addServiceBtn = new Button { Content = LanguageManager.Get("AddServiceBtn"), Classes = { "primary" } };
            addServiceBtn.Click += AddServiceForCar_Click;
            addServiceBtn.Tag = ((dynamic)item)?.Id;
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateCarFromGrid_Click;
            updateBtn.Tag = ((dynamic)item)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteCarFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item)?.Id;
            panel.Children.Add(addServiceBtn);
            panel.Children.Add(updateBtn);
            panel.Children.Add(deleteBtn);
            return panel;
        });
        CarsGrid.Columns.Add(carActionsCol);
        
        // Rebuild Services Grid
        ServicesGrid.Columns.Clear();
        ServicesGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ID"), Binding = new Binding("Id"), Width = new DataGridLength(60) });
        ServicesGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ServiceName"), Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        ServicesGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Price"), Binding = new Binding("Price", BindingMode.OneWay) { StringFormat = "{0:F2}" }, Width = new DataGridLength(100) });
        
        var serviceActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(180), IsVisible = isLoggedIn };
        serviceActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateServiceFromGrid_Click;
            updateBtn.Tag = ((dynamic)item)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteServiceFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item)?.Id;
            panel.Children.Add(updateBtn);
            panel.Children.Add(deleteBtn);
            return panel;
        });
        ServicesGrid.Columns.Add(serviceActionsCol);
        
        // Rebuild Logs Grid
        LogsGrid.Columns.Clear();
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ID"), Binding = new Binding("Id"), Width = new DataGridLength(60) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Car"), Binding = new Binding("Car"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Service"), Binding = new Binding("Service"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Date"), Binding = new Binding("DateOfService", BindingMode.OneWay) { StringFormat = "{0:d}" }, Width = new DataGridLength(120) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Mileage"), Binding = new Binding("Mileage"), Width = new DataGridLength(120) });
        
        var logActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(100), IsVisible = isLoggedIn };
        logActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteLogFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item)?.Id;
            return deleteBtn;
        });
        LogsGrid.Columns.Add(logActionsCol);
    }

    private void LoginBtn_Click(object? sender, RoutedEventArgs e)
    {
        string username = LoginNameBox.Text?.Trim() ?? "";
        string password = LoginPasswordBox.Text?.Trim() ?? "";

        if (username == "admin" && password == "admin")
        {
            LoginPanel.IsVisible = false;

            OwnerActionPanel.IsVisible = true;
            ServiceActionPanel.IsVisible = true;

            RebuildDataGrids();
            LoadOwners();
            LoadCars();
            LoadServices();
            LoadCarServices();

            ShowAlert(LanguageManager.Get("Welcome"));
        }
        else
        {
            ShowAlert(LanguageManager.Get("WrongCredentials"));
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
                results.Add($"{LanguageManager.Get("Owners").ToUpper()} • {o.FullName} • {o.Phone}");
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
                    results.Add($"{LanguageManager.Get("Cars").ToUpper()} • {c.Brand} {c.Model} ({c.RegistrationNumber})");
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
                    results.Add($"{LanguageManager.Get("ServicesLogs").ToUpper()} • {s.Name}");
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
                    results.Add($"{LanguageManager.Get("ServicesLogs").ToUpper()} • {log.Car} → {log.Service} → {log.DateOfService:yyyy-MM-dd}");
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
            Title = LanguageManager.Get("Message"),
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
        if (tab == LanguageManager.Get("Owners").ToUpper())
        {
            MainTabs.SelectedIndex = 0;
            SelectRow(OwnersGrid, o => ((string)o.FullName) == target);
        }

        // CARS
        if (tab == LanguageManager.Get("Cars").ToUpper())
        {
            MainTabs.SelectedIndex = 1;
            string reg = target.Split('(').Last().Replace(")", "").Trim();
            SelectRow(CarsGrid, c => ((string)c.RegistrationNumber) == reg);
        }

        // SERVICES & LOGS
        if (tab == LanguageManager.Get("ServicesLogs").ToUpper())
        {
            MainTabs.SelectedIndex = 2;
            
            if (target.Contains("→"))
            {
                // LOGS
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
            else
            {
                // SERVICES
                SelectRow(ServicesGrid, s => ((string)s.Name) == target);
            }
        }

        SearchBox.Text = "";
    }
}