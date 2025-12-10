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
using System.Threading.Tasks;


namespace Tund10.Avalonia;

public partial class MainWindow : Window
{
    private readonly AutoDbContext _db = new();
    private SearchResultsWindow? SearchPopup;
    private string _currentUserRole = "Viewer";

    public MainWindow()
    {
        InitializeComponent();
        _db.Database.EnsureCreated();
        SeedData.Seed(_db);

        ((TabItem)MainTabs.Items[2]!).IsVisible = false;

        LanguageManager.LoadLanguage();
        LanguageCombo.SelectedIndex = LanguageManager.CurrentLanguage switch
        {
            "RU" => 1,
            "UK" => 2,
            _ => 0
        };
        UpdateLanguage();
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadOwners();
        LoadCars();
        LoadServices();
        LoadLogs();
        LoadWorkers();
        LoadRoles();
    }

    public void LoadRoles() =>
        WorkerRoleCombo.ItemsSource = _db.Roles.Select(r => r.Name).ToList();

    private void LoadWorkers() =>
        WorkersGrid.ItemsSource = _db.Workers
            .Select(w => new { w.Id, w.Name, w.Role })
            .ToList();

    private void LoadOwners() =>
        OwnersGrid.ItemsSource = _db.Owners
            .Select(o => new { o.Id, o.FullName, o.Phone })
            .ToList();

    private void LoadCars()
    {
        var carsData = _db.Cars
            .Include(c => c.Owner)
            .ToList()
            .Select(c => new
            {
                c.Id,
                c.Brand,
                c.Model,
                c.RegistrationNumber,
                OwnerName = c.Owner != null ? c.Owner.FullName : ""
            })
            .ToList();

        CarsGrid.ItemsSource = carsData;
    }

    private void LoadServices()
    {
        ServicesGrid.ItemsSource = _db.Services
            .Select(s => new { s.Id, s.Name, Price = s.Price.ToString("F2") })
            .ToList();
    }

    private void LoadLogs()
    {
        var logsData = _db.CarServices
            .Include(cs => cs.Car)
            .Include(cs => cs.Service)
            .Include(cs => cs.Worker)
            .ToList()
            .Select(cs => new
            {
                cs.Id,
                cs.Mileage,
                cs.DateOfService,
                cs.Status,
                Car = cs.Car != null ? cs.Car.RegistrationNumber : "",
                Service = cs.Service != null ? cs.Service.Name : "",
                Worker = cs.Worker != null ? cs.Worker.Name : "Määramata",
                DateTimeDisplay = cs.DateOfService.ToString("yyyy-MM-dd HH:mm")
            })
            .ToList();

        LogsGrid.ItemsSource = logsData;
    }

    // -------------------- OWNERS --------------------

    private async void AddCarForOwner_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int ownerId) return;
        var owner = _db.Owners.Find(ownerId);
        if (owner == null) return;

        var addCarWindow = new AddCarWindow(ownerId, owner.FullName, this);
        await addCarWindow.ShowDialog(this);
        LoadCars();
    }

    private void AddOwnerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ValidateInput(OwnerNameBox.Text, OwnerPhoneBox.Text))
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        string ownerName = OwnerNameBox.Text!.Trim().ToLower();
        string ownerPhone = OwnerPhoneBox.Text!.Trim();

        if (_db.Owners.Any(o =>
            o.FullName.ToLower() == ownerName &&
            o.Phone.Trim() == ownerPhone))
        {
            ShowAlert(LanguageManager.Get("OwnerExists"));
            return;
        }

        _db.Owners.Add(new Owner
        {
            FullName = OwnerNameBox.Text!.Trim(),
            Phone = OwnerPhoneBox.Text!.Trim()
        });
        _db.SaveChanges();
        RefreshAfterChange();
        ShowAlert(LanguageManager.Get("OwnerAdded"));
        ClearInputs(OwnerNameBox, OwnerPhoneBox);
    }

    private async void UpdateOwnerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var owner = _db.Owners.Find(id);
        if (owner == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, owner, this, "Owner"));
        LoadOwners();
        LoadCars();
    }

    private void DeleteOwnerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var owner = _db.Owners.Include(o => o.Cars).FirstOrDefault(o => o.Id == id);
        if (owner?.Cars.Any() == true)
        {
            ShowAlert(LanguageManager.Get("CannotDeleteOwner"));
            return;
        }

        _db.Owners.Remove(owner!);
        _db.SaveChanges();
        RefreshAfterChange();
        ShowAlert(LanguageManager.Get("OwnerDeleted"));
    }

    // -------------------- CARS --------------------

    private async void AddServiceForCar_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int carId) return;
        var car = _db.Cars.Find(carId);
        if (car == null) return;

        await ShowUpdateDialog(new AddServiceWindow(_db, carId, car, this));
        LoadLogs();
    }

    private async void UpdateCarFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var car = _db.Cars.Find(id);
        if (car == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, car, this, "Car"));
        LoadCars();
        LoadLogs();
    }

    private void DeleteCarFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var car = _db.Cars.Include(c => c.CarServices).FirstOrDefault(c => c.Id == id);
        if (car?.CarServices.Any() == true)
        {
            ShowAlert(LanguageManager.Get("CannotDeleteCar"));
            return;
        }

        _db.Cars.Remove(car!);
        _db.SaveChanges();
        RefreshAfterChange();
        ShowAlert(LanguageManager.Get("CarDeleted"));
    }

    // -------------------- SERVICES --------------------

    private async void UpdateServiceFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var service = _db.Services.Find(id);
        if (service == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, service, this, "Service"));
        LoadServices();
        LoadLogs();
    }

    private void DeleteServiceFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var service = _db.Services.Include(s => s.CarServices)
            .FirstOrDefault(s => s.Id == id);
        if (service?.CarServices.Any() == true)
        {
            ShowAlert(LanguageManager.Get("CannotDeleteService"));
            return;
        }

        _db.Services.Remove(service!);
        _db.SaveChanges();
        RefreshAfterChange();
        ShowAlert(LanguageManager.Get("ServiceDeleted"));
    }

    private void AddServiceBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ValidateInput(ServiceNameBox.Text, ServicePriceBox.Text))
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        if (!decimal.TryParse(ServicePriceBox.Text, out decimal price) || price <= 0)
        {
            ShowAlert(LanguageManager.Get("InvalidPrice"));
            return;
        }

        string serviceName = ServiceNameBox.Text!.Trim().ToLower();

        if (_db.Services.Any(s => s.Name.ToLower() == serviceName))
        {
            ShowAlert(LanguageManager.Get("ServiceExists"));
            return;
        }

        _db.Services.Add(new Service
        {
            Name = ServiceNameBox.Text!.Trim(),
            Price = decimal.Round(price, 2)
        });
        _db.SaveChanges();
        RefreshAfterChange();
        ShowAlert(LanguageManager.Get("ServiceAdded"));
        ClearInputs(ServiceNameBox, ServicePriceBox);
    }

    // -------------------- LOGS --------------------

    private async void ChangeStatusFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var log = _db.CarServices.Find(id);
        if (log == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, log, this, "Status"));
        LoadLogs();
    }

    private void DeleteLogFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var log = _db.CarServices.Find(id);
        if (log == null) return;

        _db.CarServices.Remove(log);
        _db.SaveChanges();
        LoadLogs();
        ShowAlert(LanguageManager.Get("LogDeleted"));
    }

    // -------------------- WORKERS --------------------

    private void AddWorkerBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ValidateInput(WorkerNameBox.Text, WorkerPasswordBox.Text) || WorkerRoleCombo.SelectedIndex == -1)
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        _db.Workers.Add(new Worker
        {
            Name = WorkerNameBox.Text!.Trim(),
            Password = WorkerPasswordBox.Text!.Trim(),
            Role = WorkerRoleCombo.SelectedItem!.ToString()!
        });
        _db.SaveChanges();
        LoadWorkers();
        ShowAlert(LanguageManager.Get("WorkerAdded"));
        ClearInputs(WorkerNameBox, WorkerPasswordBox);
        WorkerRoleCombo.SelectedIndex = -1;
    }

    private async void UpdateWorkerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var worker = _db.Workers.Find(id);
        if (worker == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, worker, this, "Worker"));
        LoadWorkers();
    }

    private void DeleteWorkerFromGrid_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var worker = _db.Workers.Find(id);
        if (worker?.Name == "admin")
        {
            ShowAlert(LanguageManager.Get("CannotDeleteDefault"));
            return;
        }

        _db.Workers.Remove(worker!);
        _db.SaveChanges();
        LoadWorkers();
        ShowAlert(LanguageManager.Get("WorkerDeleted"));
    }

    private void LoginBtn_Click(object? sender, RoutedEventArgs e)
    {
        string username = LoginNameBox.Text != null ? LoginNameBox.Text.Trim() : "";
        string password = LoginPasswordBox.Text != null ? LoginPasswordBox.Text.Trim() : "";

        var worker = _db.Workers.FirstOrDefault(w =>
            w.Name == username &&
            w.Password == password);

        if (worker != null)
        {
            _currentUserRole = worker.Role;
            SetLoginState(true, worker.Name);
            ApplyRolePermissions(worker);
            LoadAllData();
            ShowAlert(LanguageManager.Format("Welcome", worker.Name));
        }
        else
        {
            ShowAlert(LanguageManager.Get("WrongCredentials"));
        }
    }

    private void LogoutBtn_Click(object? sender, RoutedEventArgs e)
    {
        _currentUserRole = "Viewer";
        SetLoginState(false, null);

        // Hide action panels for Viewer role
        OwnerActionPanel.IsVisible = false;
        ServiceActionPanel.IsVisible = false;
        WorkerActionPanel.IsVisible = false;

        ((TabItem)MainTabs.Items[0]!).IsVisible = true;
        ((TabItem)MainTabs.Items[1]!).IsVisible = true;
        ((TabItem)MainTabs.Items[2]!).IsVisible = false;
        ((TabItem)MainTabs.Items[3]!).IsVisible = true;

        UpdateActionColumns();
        LoadAllData();
    }

    private void ApplyRolePermissions(Worker worker)
    {
        var role = _db.Roles.FirstOrDefault(r => r.Name == _currentUserRole);

        bool canManageOwners = role != null && role.CanManageOwners;
        bool canManageServices = role != null && role.CanManageServices;
        bool canManageWorkers = role != null && role.CanManageWorkers;
        bool canViewOwners = role != null && role.CanViewOwners;
        bool canViewCars = role != null && role.CanViewCars;
        bool canViewServices = role != null && role.CanViewServices;
        bool canViewWorkers = role != null && role.CanViewWorkers;

        OwnerActionPanel.IsVisible = canManageOwners;
        ServiceActionPanel.IsVisible = canManageServices;
        WorkerActionPanel.IsVisible = canManageWorkers;

        ((TabItem)MainTabs.Items[0]!).IsVisible = canViewOwners;
        ((TabItem)MainTabs.Items[1]!).IsVisible = canViewCars;
        ((TabItem)MainTabs.Items[2]!).IsVisible = canViewWorkers || worker.Name == "admin";
        ((TabItem)MainTabs.Items[3]!).IsVisible = canViewServices;

        UpdateDataGridHeaders();
        UpdateActionColumns();
    }

    private void SetLoginState(bool loggedIn, string? userName)
    {
        LoginPanel.IsVisible = !loggedIn;
        LogoutBtn.IsVisible = loggedIn;
        if (!loggedIn)
        {
            LoginNameBox.Clear();
            LoginPasswordBox.Clear();
        }
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

        UpdateTabHeaders();
        UpdateInputLabels();
        UpdateDataGridHeaders();
        UpdateActionColumns();

        LoadAllData();
    }

    private void UpdateTabHeaders()
    {
        ((TabItem)MainTabs.Items[0]!).Header = LanguageManager.Get("Owners");
        ((TabItem)MainTabs.Items[1]!).Header = LanguageManager.Get("Cars");
        ((TabItem)MainTabs.Items[2]!).Header = LanguageManager.Get("Workers");
        ((TabItem)MainTabs.Items[3]!).Header = LanguageManager.Get("ServicesLogs");
    }

    private void UpdateInputLabels()
    {
        LogoutBtn.Content = LanguageManager.Get("Logout");
        WorkerNameBox.Watermark = LanguageManager.Get("WorkerName");
        WorkerPasswordBox.Watermark = LanguageManager.Get("WorkerPassword");
        ManageRolesBtn.Content = LanguageManager.Get("ManageRoles");

        OwnerNameBox.Watermark = LanguageManager.Get("Name");
        OwnerPhoneBox.Watermark = LanguageManager.Get("Phone");
        AddOwnerBtn.Content = LanguageManager.Get("Add");

        ServiceNameBox.Watermark = LanguageManager.Get("ServiceName");
        ServicePriceBox.Watermark = LanguageManager.Get("Price");
        AddServiceBtn.Content = LanguageManager.Get("Add");

        AddWorkerBtn.Content = LanguageManager.Get("Add");
    }

    private void UpdateDataGridHeaders()
    {
        ((DataGridTextColumn)OwnersGrid.Columns[0]).Header = LanguageManager.Get("ID");
        ((DataGridTextColumn)OwnersGrid.Columns[1]).Header = LanguageManager.Get("Name");
        ((DataGridTextColumn)OwnersGrid.Columns[2]).Header = LanguageManager.Get("Phone");
        ((DataGridTemplateColumn)OwnersGrid.Columns[3]).Header = LanguageManager.Get("Actions");

        ((DataGridTextColumn)CarsGrid.Columns[0]).Header = LanguageManager.Get("ID");
        ((DataGridTextColumn)CarsGrid.Columns[1]).Header = LanguageManager.Get("Brand");
        ((DataGridTextColumn)CarsGrid.Columns[2]).Header = LanguageManager.Get("Model");
        ((DataGridTextColumn)CarsGrid.Columns[3]).Header = LanguageManager.Get("RegNumber");
        ((DataGridTemplateColumn)CarsGrid.Columns[4]).Header = LanguageManager.Get("Owner");
        ((DataGridTemplateColumn)CarsGrid.Columns[5]).Header = LanguageManager.Get("Actions");

        ((DataGridTextColumn)WorkersGrid.Columns[0]).Header = LanguageManager.Get("ID");
        ((DataGridTextColumn)WorkersGrid.Columns[1]).Header = LanguageManager.Get("Name");
        ((DataGridTextColumn)WorkersGrid.Columns[2]).Header = LanguageManager.Get("WorkerRole");
        ((DataGridTemplateColumn)WorkersGrid.Columns[3]).Header = LanguageManager.Get("Actions");

        ((DataGridTextColumn)ServicesGrid.Columns[0]).Header = LanguageManager.Get("ID");
        ((DataGridTextColumn)ServicesGrid.Columns[1]).Header = LanguageManager.Get("ServiceName");
        ((DataGridTextColumn)ServicesGrid.Columns[2]).Header = LanguageManager.Get("Price");
        ((DataGridTemplateColumn)ServicesGrid.Columns[3]).Header = LanguageManager.Get("Actions");

        ((DataGridTextColumn)LogsGrid.Columns[0]).Header = LanguageManager.Get("ID");
        ((DataGridTemplateColumn)LogsGrid.Columns[1]).Header = LanguageManager.Get("Car");
        ((DataGridTemplateColumn)LogsGrid.Columns[2]).Header = LanguageManager.Get("Service");
        ((DataGridTextColumn)LogsGrid.Columns[3]).Header = LanguageManager.Get("WorkerName");
        ((DataGridTextColumn)LogsGrid.Columns[4]).Header = LanguageManager.Get("Date");
        ((DataGridTextColumn)LogsGrid.Columns[5]).Header = LanguageManager.Get("Mileage");
        ((DataGridTextColumn)LogsGrid.Columns[6]).Header = LanguageManager.Get("Status");
        ((DataGridTemplateColumn)LogsGrid.Columns[7]).Header = LanguageManager.Get("Actions");
        ((DataGridTemplateColumn)LogsGrid.Columns[8]).Header = LanguageManager.Get("Status");
    }

    private void UpdateActionColumns()
    {
        var role = _db.Roles.FirstOrDefault(r => r.Name == _currentUserRole);

        bool canManageOwners = role != null && role.CanManageOwners;
        bool canManageCars = role != null && role.CanManageCars;
        bool canChangeStatus = role != null && role.CanChangeStatus;
        bool canManageServices = role != null && role.CanManageServices;
        bool canManageWorkers = role != null && role.CanManageWorkers;

        SetupOwnerActions(canManageOwners);
        SetupCarActions(canManageCars);
        SetupServiceActions(canManageServices);
        SetupLogActions(canManageServices, canChangeStatus, canManageCars);

        SetupWorkerActions(canManageWorkers);
    }

    private void SetupOwnerActions(bool canManage)
    {
        var col = (DataGridTemplateColumn)OwnersGrid.Columns[3];
        col.IsVisible = canManage;
        col.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;
            AddButtonWithHandler(panel, LanguageManager.Get("AddCarBtn"), "primary",
                (EventHandler<RoutedEventArgs>)((s, e) => AddCarForOwner_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("UpdateBtn"), "warning",
                (EventHandler<RoutedEventArgs>)((s, e) => UpdateOwnerFromGrid_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("DeleteBtn"), "danger",
                (EventHandler<RoutedEventArgs>)((s, e) => DeleteOwnerFromGrid_Click(s, e)), itemData.Id);
            return panel;
        });
    }

    private void SetupCarActions(bool canManage)
    {
        // Owner column (clickable link)
        var ownerCol = (DataGridTemplateColumn)CarsGrid.Columns[4];
        ownerCol.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            dynamic itemData = item!;
            var btn = new Button
            {
                Content = new TextBlock
                {
                    Text = itemData.OwnerName,
                    Foreground = new SolidColorBrush(Colors.Blue)
                },
                Background = new SolidColorBrush(Colors.Transparent),
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                Cursor = new Cursor(StandardCursorType.Hand)
            };
            btn.Click += (s, e) => HandleOwnerLinkClick(itemData.OwnerName);
            return btn;
        });

        // Actions column
        var col = (DataGridTemplateColumn)CarsGrid.Columns[5];
        col.IsVisible = canManage;
        col.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;
            AddButtonWithHandler(panel, LanguageManager.Get("AddServiceBtn"), "primary",
                (EventHandler<RoutedEventArgs>)((s, e) => AddServiceForCar_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("UpdateBtn"), "warning",
                (EventHandler<RoutedEventArgs>)((s, e) => UpdateCarFromGrid_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("DeleteBtn"), "danger",
                (EventHandler<RoutedEventArgs>)((s, e) => DeleteCarFromGrid_Click(s, e)), itemData.Id);
            return panel;
        });
    }

    private void SetupServiceActions(bool canManage)
    {
        var col = (DataGridTemplateColumn)ServicesGrid.Columns[3];
        col.IsVisible = canManage;
        col.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;
            AddButtonWithHandler(panel, LanguageManager.Get("UpdateBtn"), "warning",
                (EventHandler<RoutedEventArgs>)((s, e) => UpdateServiceFromGrid_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("DeleteBtn"), "danger",
                (EventHandler<RoutedEventArgs>)((s, e) => DeleteServiceFromGrid_Click(s, e)), itemData.Id);
            return panel;
        });
    }

    private async void AssignWorkerToLog_Click(object? sender, RoutedEventArgs e)
    {
        if (GetTag(sender) is not int id) return;
        var log = _db.CarServices.Include(l => l.Worker).FirstOrDefault(l => l.Id == id);
        if (log == null) return;

        await ShowUpdateDialog(new UpdateWindow(_db, log, this, "AssignWorker"));
        LoadLogs();
    }

    private void SetupLogActions(bool canManageServices, bool canChangeStatus, bool canManageCars)
    {
        // Car column (clickable link)
        var carCol = (DataGridTemplateColumn)LogsGrid.Columns[1];
        carCol.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            dynamic itemData = item!;
            var btn = new Button
            {
                Content = new TextBlock
                {
                    Text = itemData.Car,
                    Foreground = new SolidColorBrush(Colors.Blue)
                },
                Background = new SolidColorBrush(Colors.Transparent),
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                Cursor = new Cursor(StandardCursorType.Hand)
            };
            btn.Click += (s, e) => HandleCarLinkClick(itemData.Car);
            return btn;
        });

        // Service column (clickable link)
        var serviceCol = (DataGridTemplateColumn)LogsGrid.Columns[2];
        serviceCol.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            dynamic itemData = item!;
            var btn = new Button
            {
                Content = new TextBlock
                {
                    Text = itemData.Service,
                    Foreground = new SolidColorBrush(Colors.Blue)
                },
                Background = new SolidColorBrush(Colors.Transparent),
                Padding = new Thickness(0),
                BorderThickness = new Thickness(0),
                Cursor = new Cursor(StandardCursorType.Hand)
            };
            btn.Click += (s, e) => HandleServiceLinkClick(itemData.Service);
            return btn;
        });

        // Column 7 - Worker Name and Delete buttons (Tegevus)
        var col = (DataGridTemplateColumn)LogsGrid.Columns[7];
        col.IsVisible = canManageServices || canManageCars;
        col.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;

            AddButtonWithHandler(panel, LanguageManager.Get("WorkerName"), "primary",
                (EventHandler<RoutedEventArgs>)((s, e) => AssignWorkerToLog_Click(s, e)), itemData.Id, 100);
            AddButtonWithHandler(panel, LanguageManager.Get("DeleteBtn"), "danger",
                (EventHandler<RoutedEventArgs>)((s, e) => DeleteLogFromGrid_Click(s, e)), itemData.Id, 70);

            return panel;
        });

        // Column 8 - Status button (Tegevus 2)
        var col2 = (DataGridTemplateColumn)LogsGrid.Columns[8];
        col2.IsVisible = canChangeStatus;
        col2.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;

            AddButtonWithHandler(panel, LanguageManager.Get("Status"), "purple",
                (EventHandler<RoutedEventArgs>)((s, e) => ChangeStatusFromGrid_Click(s, e)), itemData.Id, 70);

            return panel;
        });

        LogsGrid.LoadingRow -= LogsGrid_LoadingRow;
        LogsGrid.LoadingRow += LogsGrid_LoadingRow;
    }

    private void LogsGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (e.Row.DataContext is not null)
        {
            dynamic data = e.Row.DataContext;
            string status = data.Status;
            e.Row.Background = status switch
            {
                "Alustamata" => new SolidColorBrush(Color.FromRgb(255, 240, 240)),
                "Pooleli" => new SolidColorBrush(Color.FromRgb(255, 250, 205)),
                "Valmis" => new SolidColorBrush(Color.FromRgb(220, 255, 220)),
                _ => Brushes.White
            };
        }
    }

    private void SetupWorkerActions(bool canManage)
    {
        var col = (DataGridTemplateColumn)WorkersGrid.Columns[3];
        col.IsVisible = canManage;
        col.CellTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var panel = CreateActionPanel();
            dynamic itemData = item!;
            AddButtonWithHandler(panel, LanguageManager.Get("UpdateBtn"), "warning",
                (EventHandler<RoutedEventArgs>)((s, e) => UpdateWorkerFromGrid_Click(s, e)), itemData.Id);
            AddButtonWithHandler(panel, LanguageManager.Get("DeleteBtn"), "danger",
                (EventHandler<RoutedEventArgs>)((s, e) => DeleteWorkerFromGrid_Click(s, e)), itemData.Id);
            return panel;
        });
    }

    private StackPanel CreateActionPanel() =>
        new() { Orientation = Orientation.Horizontal, Spacing = 5 };

    private void AddButtonWithHandler(StackPanel panel, string content, string classes,
    EventHandler<RoutedEventArgs> handler, int tag, int width = 0)
    {
        var btn = new Button
        {
            Content = content,
            Classes = { classes },
            Tag = tag
        };
        if (width > 0) btn.Width = width;
        btn.Click += handler;
        panel.Children.Add(btn);
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
        foreach (dynamic o in OwnersGrid.ItemsSource ?? Enumerable.Empty<object>())
        {
            if (((string)o.FullName).ToLower().Contains(query) ||
                ((string)o.Phone).ToLower().Contains(query))
            {
                results.Add($"{LanguageManager.Get("Owners").ToUpper()} • {o.FullName} • {o.Phone}");
            }
        }

        // CARS
        if (results.Count < 3)
        {
            foreach (dynamic c in CarsGrid.ItemsSource ?? Enumerable.Empty<object>())
            {
                if (((string)c.Brand).ToLower().Contains(query) ||
                    ((string)c.Model).ToLower().Contains(query) ||
                    ((string)c.RegistrationNumber).ToLower().Contains(query) ||
                    ((string)c.OwnerName).ToLower().Contains(query))
                {
                    results.Add($"{LanguageManager.Get("Cars").ToUpper()} • {c.Brand} {c.Model} ({c.RegistrationNumber})");
                }
            }
        }

        // SERVICES
        if (results.Count < 3)
        {
            foreach (dynamic s in ServicesGrid.ItemsSource ?? Enumerable.Empty<object>())
            {
                if (((string)s.Name).ToLower().Contains(query))
                {
                    results.Add($"{LanguageManager.Get("ServicesLogs").ToUpper()} • {s.Name}");
                }
            }
        }

        // LOGS
        if (results.Count < 3)
        {
            foreach (dynamic log in LogsGrid.ItemsSource ?? Enumerable.Empty<object>())
            {
                if (((string)log.Car).ToLower().Contains(query) ||
                    ((string)log.Service).ToLower().Contains(query) ||
                    log.Mileage.ToString().Contains(query))
                {
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

        SearchPopup ??= new SearchResultsWindow(this) { ShowActivated = false };

        var screenPos = SearchBox.PointToScreen(new Point(0, SearchBox.Bounds.Height));
        SearchPopup.Position = new PixelPoint((int)screenPos.X, (int)screenPos.Y);
        SearchPopup.SetResults(results);
        SearchPopup.Show(this);
    }

    public async void HandleSearchSelection(string line)
    {
        if (!line.Contains("•")) return;

        string[] parts = line.Split('•');
        string tab = parts[0].Trim().ToUpper();
        string target = parts[1].Trim();

        if (tab == LanguageManager.Get("Owners").ToUpper())
        {
            MainTabs.SelectedIndex = 0;
            await Task.Delay(100);
            SelectRowAndFocus(OwnersGrid, o => ((string)o.FullName) == target);
        }
        else if (tab == LanguageManager.Get("Cars").ToUpper())
        {
            MainTabs.SelectedIndex = 1;
            await Task.Delay(100);
            string reg = target.Split('(').Last().Replace(")", "").Trim();
            SelectRowAndFocus(CarsGrid, c => ((string)c.RegistrationNumber) == reg);
        }
        else if (tab == LanguageManager.Get("ServicesLogs").ToUpper())
        {
            MainTabs.SelectedIndex = 3;
            await Task.Delay(100);

            if (target.Contains("→"))
            {
                var parts2 = target.Split('→', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts2.Length >= 3)
                {
                    var carName = parts2[0];
                    var serviceName = parts2[1];
                    var date = DateTime.Parse(parts2[2]);
                    SelectRowAndFocus(LogsGrid, l =>
                        ((string)l.Car).Equals(carName, StringComparison.OrdinalIgnoreCase) &&
                        ((string)l.Service).Equals(serviceName, StringComparison.OrdinalIgnoreCase) &&
                        ((DateTime)l.DateOfService).Date == date.Date);
                }
            }
            else
            {
                SelectRowAndFocus(ServicesGrid, s => ((string)s.Name) == target);
            }
        }

        SearchBox.Text = "";
    }

    private void SelectRowAndFocus(DataGrid grid, Func<dynamic, bool> match)
    {
        var itemsSource = grid.ItemsSource as IEnumerable<object>;
        if (itemsSource == null) return;

        var itemList = itemsSource.ToList();
        int index = 0;
        foreach (var item in itemList)
        {
            if (match((dynamic)item))
            {
                grid.SelectedIndex = index;
                grid.ScrollIntoView(item, grid.Columns[0]);
                grid.Focus();
                break;
            }
            index++;
        }
    }

    private async Task ShowUpdateDialog(Window dialog) =>
        await dialog.ShowDialog(this);

    private void RefreshAfterChange()
    {
        LoadOwners();
        LoadCars();
        LoadServices();
        LoadLogs();
        LoadWorkers();
    }

    private void ClearInputs(params TextBox[] inputs)
    {
        foreach (var tb in inputs)
            tb.Clear();
    }

    private bool ValidateInput(params string?[] inputs) =>
        inputs.Any(i => string.IsNullOrWhiteSpace(i));

    private int? GetTag(object? sender) =>
        (sender as Button)?.Tag as int?;

    public async void ShowAlert(string message)
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

        stack.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        });

        var okButton = new Button
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        okButton.Click += (_, _) => dlg.Close();
        stack.Children.Add(okButton);
        dlg.Content = stack;

        await dlg.ShowDialog(this);
    }

    private async void HandleOwnerLinkClick(string ownerName)
    {
        MainTabs.SelectedIndex = 0;
        await Task.Delay(100);
        SelectRowAndFocus(OwnersGrid, o => ((string)o.FullName) == ownerName);
    }

    private async void HandleCarLinkClick(string registrationNumber)
    {
        MainTabs.SelectedIndex = 1;
        await Task.Delay(100);
        SelectRowAndFocus(CarsGrid, c => ((string)c.RegistrationNumber) == registrationNumber);
    }

    private async void HandleServiceLinkClick(string serviceName)
    {
        MainTabs.SelectedIndex = 3;
        await Task.Delay(100);
        SelectRowAndFocus(ServicesGrid, s => ((string)s.Name) == serviceName);
    }

    private async void ManageRolesBtn_Click(object? sender, RoutedEventArgs e) =>
        await ShowUpdateDialog(new ManageRolesWindow(_db, this));
}