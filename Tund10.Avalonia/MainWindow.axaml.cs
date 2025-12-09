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
    private string _currentUserRole = "Owner";

    public MainWindow()
    {
        InitializeComponent();
        _db.Database.EnsureCreated();
        SeedData.Seed(_db);

        _currentUserRole = "Viewer";
        ((TabItem)MainTabs.Items[2]!).IsVisible = false;

        LanguageManager.LoadLanguage();
        LanguageCombo.SelectedIndex = LanguageManager.CurrentLanguage switch
        {
            "RU" => 1,
            "UK" => 2,
            _ => 0
        };
        UpdateLanguage();

        LoadOwners();
        LoadCars();
        LoadServices();
        LoadCarServices();
        LoadWorkers();
        LoadRoles();
    }

    // -------------------- LOAD METHODS --------------------

    private void LoadRoles()
    {
        WorkerRoleCombo.ItemsSource = _db.Roles.Select(r => r.Name).ToList();
    }

    private void LoadWorkers()
    {
        var workers = _db.Workers
            .Select(w => new { w.Id, w.Name, w.Role })
            .ToList();

        WorkersGrid.ItemsSource = workers;
    }

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
                OwnerName = c.Owner!.FullName
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
                cs.Status,
                Car = cs.Car!.RegistrationNumber,
                Service = cs.Service!.Name,
                DateTimeDisplay = cs.DateOfService.ToString("yyyy-MM-dd HH:mm")
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

        var hourCombo = new ComboBox { Width = 350, PlaceholderText = LanguageManager.Get("SelectService"), HorizontalAlignment = HorizontalAlignment.Left };
        for (int h = 0; h < 24; h++)
            hourCombo.Items.Add($"{h:D2}:00");
        hourCombo.SelectedIndex = DateTime.Now.Hour;
        stack.Children.Add(hourCombo);

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
                hourCombo.SelectedIndex == -1 ||
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

            DateTime dateTime = datePicker.SelectedDate.Value.Date.AddHours(hourCombo.SelectedIndex);

            bool exists = _db.CarServices.Any(cs => cs.DateOfService == dateTime);

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
                DateOfService = dateTime
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

    // -------------------- MAINTENANCE LOGS --------------------

    private async void ChangeStatusFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int logId)
            return;

        var log = _db.CarServices.Find(logId);
        if (log == null) return;

        var dlg = new Window
        {
            Width = 300,
            Height = 200,
            Title = "Change Status",
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };
        var statusCombo = new ComboBox { Width = 250 };
        statusCombo.Items.Add("Not Started");
        statusCombo.Items.Add("In Progress");
        statusCombo.Items.Add("Done");
        statusCombo.SelectedItem = log.Status;
        stack.Children.Add(statusCombo);

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
            log.Status = statusCombo.SelectedItem?.ToString() ?? "Not Started";
            _db.SaveChanges();
            LoadCarServices();
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
        LanguageManager.CurrentLanguage = LanguageCombo.SelectedIndex switch
        {
            0 => "EE",
            1 => "RU",
            2 => "UK",
            _ => "EE"
        };
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
        ((TabItem)MainTabs.Items[2]!).Header = "Workers";
        ((TabItem)MainTabs.Items[3]!).Header = LanguageManager.Get("ServicesLogs");

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
        LoadWorkers();
    }

    private void RebuildDataGrids()
    {
        bool isLoggedIn = !LoginPanel.IsVisible;
        var currentRole = _db.Roles.FirstOrDefault(r => r.Name == _currentUserRole);
        bool canManage = currentRole?.CanManageOwners ?? false;
        bool canChangeStatus = currentRole?.CanChangeStatus ?? false;

        // Rebuild Owners Grid
        OwnersGrid.Columns.Clear();
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("ID"), Binding = new Binding("Id"), Width = new DataGridLength(60) });
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Name"), Binding = new Binding("FullName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        OwnersGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Phone"), Binding = new Binding("Phone"), Width = new DataGridLength(150) });

        var ownerActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(320), IsVisible = canManage };
        ownerActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var addCarBtn = new Button { Content = LanguageManager.Get("AddCarBtn"), Classes = { "primary" } };
            addCarBtn.Click += AddCarForOwner_Click;
            addCarBtn.Tag = ((dynamic)item!)?.Id;
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateOwnerFromGrid_Click;
            updateBtn.Tag = ((dynamic)item!)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteOwnerFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item!)?.Id;
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

        var carActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(360), IsVisible = canManage };
        carActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var addServiceBtn = new Button { Content = LanguageManager.Get("AddServiceBtn"), Classes = { "primary" } };
            addServiceBtn.Click += AddServiceForCar_Click;
            addServiceBtn.Tag = ((dynamic)item!)?.Id;
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateCarFromGrid_Click;
            updateBtn.Tag = ((dynamic)item!)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteCarFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item!)?.Id;
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

        var serviceActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(180), IsVisible = canManage };
        serviceActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateServiceFromGrid_Click;
            updateBtn.Tag = ((dynamic)item!)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteServiceFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item!)?.Id;
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
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Date"), Binding = new Binding("DateTimeDisplay"), Width = new DataGridLength(160) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = LanguageManager.Get("Mileage"), Binding = new Binding("Mileage"), Width = new DataGridLength(120) });
        LogsGrid.Columns.Add(new DataGridTextColumn { Header = "Status", Binding = new Binding("Status"), Width = new DataGridLength(120) });

        var logActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(200), IsVisible = canChangeStatus };
        logActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var statusBtn = new Button { Content = "Status", Width = 80 };
            statusBtn.Click += ChangeStatusFromGrid_Click;
            statusBtn.Tag = ((dynamic)item!)?.Id;
            panel.Children.Add(statusBtn);
            if (canManage)
            {
                var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" }, Width = 80 };
                deleteBtn.Click += DeleteLogFromGrid_Click;
                deleteBtn.Tag = ((dynamic)item!)?.Id;
                panel.Children.Add(deleteBtn);
            }
            return panel;
        });
        LogsGrid.Columns.Add(logActionsCol);
        
        LogsGrid.LoadingRow += (s, e) =>
        {
            if (e.Row.DataContext is not null)
            {
                dynamic item = e.Row.DataContext;
                string status = item.Status;
                e.Row.Background = status switch
                {
                    "Not Started" => new SolidColorBrush(Color.FromRgb(255, 240, 240)),
                    "In Progress" => new SolidColorBrush(Color.FromRgb(255, 250, 205)),
                    "Done" => new SolidColorBrush(Color.FromRgb(220, 255, 220)),
                    _ => Brushes.White
                };
            }
        };

        // Rebuild Workers Grid
        WorkersGrid.Columns.Clear();
        WorkersGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = new DataGridLength(60) });
        WorkersGrid.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        WorkersGrid.Columns.Add(new DataGridTextColumn { Header = "Role", Binding = new Binding("Role"), Width = new DataGridLength(150) });

        var workerActionsCol = new DataGridTemplateColumn { Header = LanguageManager.Get("Actions"), Width = new DataGridLength(180), IsVisible = currentRole?.CanManageWorkers ?? false };
        workerActionsCol.CellTemplate = new FuncDataTemplate<object>((item, ns) =>
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var updateBtn = new Button { Content = LanguageManager.Get("UpdateBtn"), Classes = { "warning" } };
            updateBtn.Click += UpdateWorkerFromGrid_Click;
            updateBtn.Tag = ((dynamic)item!)?.Id;
            var deleteBtn = new Button { Content = LanguageManager.Get("DeleteBtn"), Classes = { "danger" } };
            deleteBtn.Click += DeleteWorkerFromGrid_Click;
            deleteBtn.Tag = ((dynamic)item!)?.Id;
            panel.Children.Add(updateBtn);
            panel.Children.Add(deleteBtn);
            return panel;
        });
        WorkersGrid.Columns.Add(workerActionsCol);
    }

    private void AddWorkerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(WorkerNameBox.Text) ||
            string.IsNullOrWhiteSpace(WorkerPasswordBox.Text) ||
            WorkerRoleCombo.SelectedIndex == -1)
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        _db.Workers.Add(new Worker
        {
            Name = WorkerNameBox.Text.Trim(),
            Password = WorkerPasswordBox.Text.Trim(),
            Role = WorkerRoleCombo.SelectedItem!.ToString()!
        });
        _db.SaveChanges();
        LoadWorkers();
        ShowAlert("Worker added successfully!");
        WorkerNameBox.Clear();
        WorkerPasswordBox.Clear();
        WorkerRoleCombo.SelectedIndex = -1;
    }

    private async void UpdateWorkerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int workerId)
            return;

        var worker = _db.Workers.Find(workerId);
        if (worker == null) return;

        var dlg = new Window
        {
            Width = 400,
            Height = 300,
            Title = "Update Worker",
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        var nameBox = new TextBox { Width = 350, Text = worker.Name, Watermark = "Name" };
        var passwordBox = new TextBox { Width = 350, Text = worker.Password, Watermark = "Password" };
        var roleCombo = new ComboBox { Width = 350, ItemsSource = _db.Roles.Select(r => r.Name).ToList() };
        roleCombo.SelectedItem = worker.Role;

        stack.Children.Add(nameBox);
        stack.Children.Add(passwordBox);
        stack.Children.Add(roleCombo);

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
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text) || roleCombo.SelectedItem == null)
            {
                ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            if (worker.Name == "admin" || worker.Name == "everyone")
            {
                ShowAlert("Cannot modify default workers.");
                return;
            }

            worker.Name = nameBox.Text.Trim();
            worker.Password = passwordBox.Text.Trim();
            worker.Role = roleCombo.SelectedItem.ToString()!;
            _db.SaveChanges();
            LoadWorkers();
            ShowAlert("Worker updated successfully!");
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

    private void DeleteWorkerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int workerId)
            return;

        var worker = _db.Workers.Find(workerId);
        if (worker == null) return;

        if (worker.Name == "admin" || worker.Name == "everyone")
        {
            ShowAlert("Cannot delete default workers.");
            return;
        }

        _db.Workers.Remove(worker);
        _db.SaveChanges();
        LoadWorkers();
        ShowAlert("Worker deleted successfully!");
    }

    private void LoginBtn_Click(object? sender, RoutedEventArgs e)
    {
        string username = LoginNameBox.Text?.Trim() ?? "";
        string password = LoginPasswordBox.Text?.Trim() ?? "";

        var worker = _db.Workers.FirstOrDefault(w => w.Name == username && w.Password == password);
        if (worker != null)
        {
            _currentUserRole = worker.Role;
            LoginPanel.IsVisible = false;
            LogoutBtn.IsVisible = true;

            var role = _db.Roles.FirstOrDefault(r => r.Name == _currentUserRole);
            OwnerActionPanel.IsVisible = role?.CanManageOwners ?? false;
            ServiceActionPanel.IsVisible = role?.CanManageServices ?? false;
            WorkerActionPanel.IsVisible = role?.CanManageWorkers ?? false;

            ((TabItem)MainTabs.Items[2]!).IsVisible = worker.Name == "admin";

            RebuildDataGrids();
            LoadOwners();
            LoadCars();
            LoadServices();
            LoadCarServices();
            LoadWorkers();

            ShowAlert($"Welcome, {worker.Name}!");
        }
        else
        {
            ShowAlert(LanguageManager.Get("WrongCredentials"));
        }
    }

    private void LogoutBtn_Click(object? sender, RoutedEventArgs e)
    {
        _currentUserRole = "Viewer";
        LoginPanel.IsVisible = true;
        LogoutBtn.IsVisible = false;
        LoginNameBox.Clear();
        LoginPasswordBox.Clear();

        OwnerActionPanel.IsVisible = false;
        ServiceActionPanel.IsVisible = false;
        WorkerActionPanel.IsVisible = false;
        ((TabItem)MainTabs.Items[2]!).IsVisible = false;

        RebuildDataGrids();
        LoadOwners();
        LoadCars();
        LoadServices();
        LoadCarServices();
        LoadWorkers();
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

    private async void ManageRolesBtn_Click(object? sender, RoutedEventArgs e)
    {
        var dlg = new Window
        {
            Width = 600,
            Height = 500,
            Title = "Manage Roles",
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var mainStack = new StackPanel { Margin = new Thickness(20), Spacing = 15 };

        var rolesGrid = new DataGrid { Height = 300, AutoGenerateColumns = false, IsReadOnly = true };
        rolesGrid.Columns.Add(new DataGridTextColumn { Header = "Role", Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        rolesGrid.ItemsSource = _db.Roles.ToList();
        mainStack.Children.Add(rolesGrid);

        var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
        
        var addBtn = new Button { Content = "Add Role", Width = 100 };
        addBtn.Click += async (_, _) =>
        {
            var addDlg = new Window { Width = 400, Height = 400, Title = "Add Role", WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var stack = new StackPanel { Margin = new Thickness(20), Spacing = 10 };
            var nameBox = new TextBox { Watermark = "Role Name", Width = 350 };
            var ownersCheck = new CheckBox { Content = "Can Manage Owners" };
            var carsCheck = new CheckBox { Content = "Can Manage Cars" };
            var servicesCheck = new CheckBox { Content = "Can Manage Services" };
            var statusCheck = new CheckBox { Content = "Can Change Status" };
            var workersCheck = new CheckBox { Content = "Can Manage Workers" };
            stack.Children.Add(nameBox);
            stack.Children.Add(ownersCheck);
            stack.Children.Add(carsCheck);
            stack.Children.Add(servicesCheck);
            stack.Children.Add(statusCheck);
            stack.Children.Add(workersCheck);
            var saveBtn = new Button { Content = "Save", Width = 100, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) };
            saveBtn.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    ShowAlert("Please enter role name.");
                    return;
                }
                _db.Roles.Add(new Role
                {
                    Name = nameBox.Text.Trim(),
                    CanManageOwners = ownersCheck.IsChecked ?? false,
                    CanManageCars = carsCheck.IsChecked ?? false,
                    CanManageServices = servicesCheck.IsChecked ?? false,
                    CanChangeStatus = statusCheck.IsChecked ?? false,
                    CanManageWorkers = workersCheck.IsChecked ?? false
                });
                _db.SaveChanges();
                rolesGrid.ItemsSource = _db.Roles.ToList();
                LoadRoles();
                addDlg.Close();
            };
            stack.Children.Add(saveBtn);
            addDlg.Content = stack;
            await addDlg.ShowDialog(dlg);
        };

        var editBtn = new Button { Content = "Edit Role", Width = 100 };
        editBtn.Click += async (_, _) =>
        {
            if (rolesGrid.SelectedItem is not Role role) return;
            var editDlg = new Window { Width = 400, Height = 400, Title = "Edit Role", WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var stack = new StackPanel { Margin = new Thickness(20), Spacing = 10 };
            var nameBox = new TextBox { Text = role.Name, Watermark = "Role Name", Width = 350 };
            var ownersCheck = new CheckBox { Content = "Can Manage Owners", IsChecked = role.CanManageOwners };
            var carsCheck = new CheckBox { Content = "Can Manage Cars", IsChecked = role.CanManageCars };
            var servicesCheck = new CheckBox { Content = "Can Manage Services", IsChecked = role.CanManageServices };
            var statusCheck = new CheckBox { Content = "Can Change Status", IsChecked = role.CanChangeStatus };
            var workersCheck = new CheckBox { Content = "Can Manage Workers", IsChecked = role.CanManageWorkers };
            stack.Children.Add(nameBox);
            stack.Children.Add(ownersCheck);
            stack.Children.Add(carsCheck);
            stack.Children.Add(servicesCheck);
            stack.Children.Add(statusCheck);
            stack.Children.Add(workersCheck);
            var saveBtn = new Button { Content = "Save", Width = 100, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) };
            saveBtn.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    ShowAlert("Please enter role name.");
                    return;
                }
                role.Name = nameBox.Text.Trim();
                role.CanManageOwners = ownersCheck.IsChecked ?? false;
                role.CanManageCars = carsCheck.IsChecked ?? false;
                role.CanManageServices = servicesCheck.IsChecked ?? false;
                role.CanChangeStatus = statusCheck.IsChecked ?? false;
                role.CanManageWorkers = workersCheck.IsChecked ?? false;
                _db.SaveChanges();
                rolesGrid.ItemsSource = _db.Roles.ToList();
                LoadRoles();
                editDlg.Close();
            };
            stack.Children.Add(saveBtn);
            editDlg.Content = stack;
            await editDlg.ShowDialog(dlg);
        };

        var deleteBtn = new Button { Content = "Delete Role", Width = 100 };
        deleteBtn.Click += (_, _) =>
        {
            if (rolesGrid.SelectedItem is not Role role) return;
            if (_db.Workers.Any(w => w.Role == role.Name))
            {
                ShowAlert("Cannot delete role with assigned workers.");
                return;
            }
            _db.Roles.Remove(role);
            _db.SaveChanges();
            rolesGrid.ItemsSource = _db.Roles.ToList();
            LoadRoles();
        };

        btnPanel.Children.Add(addBtn);
        btnPanel.Children.Add(editBtn);
        btnPanel.Children.Add(deleteBtn);
        mainStack.Children.Add(btnPanel);

        var closeBtn = new Button { Content = "Close", Width = 100, HorizontalAlignment = HorizontalAlignment.Center };
        closeBtn.Click += (_, _) => dlg.Close();
        mainStack.Children.Add(closeBtn);

        dlg.Content = mainStack;
        await dlg.ShowDialog(this);
    }
}