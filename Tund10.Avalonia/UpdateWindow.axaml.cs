using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Tund10.Avalonia.Models;

namespace Tund10.Avalonia;

public partial class UpdateWindow : Window
{
    private readonly AutoDbContext _db;
    private readonly MainWindow _mainWindow;
    private Action? _onSave;
    private readonly string _entityType;

    public UpdateWindow()
    {
        InitializeComponent();
        _db = null!;
        _mainWindow = null!;
        _entityType = null!;
    }

    public UpdateWindow(AutoDbContext db, object entity, MainWindow mainWindow, string entityType)
    {
        InitializeComponent();
        _db = db;
        _mainWindow = mainWindow;
        _entityType = entityType;

        switch (entityType)
        {
            case "Owner":
                SetupOwner((Owner)entity);
                break;
            case "Car":
                SetupCar((Car)entity);
                break;
            case "Service":
                SetupService((Service)entity);
                break;
            case "Worker":
                SetupWorker((Worker)entity);
                break;
            case "Status":
                SetupStatus((CarService)entity);
                break;
            case "AssignWorker":
                SetupWorkerAssignment((CarService)entity);
                break;
        }

        SaveBtn.Content = LanguageManager.Get("Save");
        CancelBtn.Content = LanguageManager.Get("Cancel");
    }

    private void SetupOwner(Owner owner)
    {
        Title = LanguageManager.Get("UpdateOwner");

        var nameLabel = new TextBlock
        {
            Text = LanguageManager.Get("Name"),
            FontWeight = FontWeight.Bold
        };
        var nameBox = new TextBox { Width = 350, Text = owner.FullName };

        var phoneLabel = new TextBlock
        {
            Text = LanguageManager.Get("Phone"),
            FontWeight = FontWeight.Bold
        };
        var phoneBox = new TextBox { Width = 350, Text = owner.Phone };

        ContentPanel.Children.Add(nameLabel);
        ContentPanel.Children.Add(nameBox);
        ContentPanel.Children.Add(phoneLabel);
        ContentPanel.Children.Add(phoneBox);

        _onSave = () =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(phoneBox.Text))
            {
                _mainWindow.ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            owner.FullName = nameBox.Text.Trim();
            owner.Phone = phoneBox.Text.Trim();
            _db.SaveChanges();
            _mainWindow.ShowAlert(LanguageManager.Get("OwnerUpdated"));
            Close();
        };
    }

    private void SetupCar(Car car)
    {
        Title = LanguageManager.Get("UpdateCar");

        var brandLabel = new TextBlock
        {
            Text = LanguageManager.Get("Brand"),
            FontWeight = FontWeight.Bold
        };
        var brandBox = new TextBox { Width = 350, Text = car.Brand };

        var modelLabel = new TextBlock
        {
            Text = LanguageManager.Get("Model"),
            FontWeight = FontWeight.Bold
        };
        var modelBox = new TextBox { Width = 350, Text = car.Model };

        var regLabel = new TextBlock
        {
            Text = LanguageManager.Get("RegNumber"),
            FontWeight = FontWeight.Bold
        };
        var regBox = new TextBox { Width = 350, Text = car.RegistrationNumber };

        var ownerLabel = new TextBlock
        {
            Text = LanguageManager.Get("Owner"),
            FontWeight = FontWeight.Bold
        };
        var ownerCombo = new ComboBox { Width = 350 };
        ownerCombo.ItemsSource = _db.Owners.ToList();
        ownerCombo.ItemTemplate = new FuncDataTemplate<Owner>((o, _) => new TextBlock { Text = o?.FullName });
        ownerCombo.SelectedItem = _db.Owners.Find(car.OwnerId);

        ContentPanel.Children.Add(brandLabel);
        ContentPanel.Children.Add(brandBox);
        ContentPanel.Children.Add(modelLabel);
        ContentPanel.Children.Add(modelBox);
        ContentPanel.Children.Add(regLabel);
        ContentPanel.Children.Add(regBox);
        ContentPanel.Children.Add(ownerLabel);
        ContentPanel.Children.Add(ownerCombo);

        _onSave = () =>
        {
            if (string.IsNullOrWhiteSpace(brandBox.Text) ||
                string.IsNullOrWhiteSpace(modelBox.Text) ||
                string.IsNullOrWhiteSpace(regBox.Text) ||
                ownerCombo.SelectedItem is not Owner selectedOwner)
            {
                _mainWindow.ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            car.Brand = brandBox.Text.Trim();
            car.Model = modelBox.Text.Trim();
            car.RegistrationNumber = regBox.Text.Trim().ToUpper();
            car.OwnerId = selectedOwner.Id;
            _db.SaveChanges();
            _mainWindow.ShowAlert(LanguageManager.Get("CarUpdated"));
            Close();
        };
    }

    private void SetupService(Service service)
    {
        Title = LanguageManager.Get("UpdateService");

        var nameLabel = new TextBlock
        {
            Text = LanguageManager.Get("ServiceName"),
            FontWeight = FontWeight.Bold
        };
        var nameBox = new TextBox { Width = 350, Text = service.Name };

        var priceLabel = new TextBlock
        {
            Text = LanguageManager.Get("Price"),
            FontWeight = FontWeight.Bold
        };
        var priceBox = new TextBox { Width = 350, Text = service.Price.ToString("F2") };

        ContentPanel.Children.Add(nameLabel);
        ContentPanel.Children.Add(nameBox);
        ContentPanel.Children.Add(priceLabel);
        ContentPanel.Children.Add(priceBox);

        _onSave = () =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(priceBox.Text))
            {
                _mainWindow.ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            if (!decimal.TryParse(priceBox.Text, out decimal price) || price <= 0)
            {
                _mainWindow.ShowAlert(LanguageManager.Get("InvalidPrice"));
                return;
            }

            service.Name = nameBox.Text.Trim();
            service.Price = price;
            _db.SaveChanges();
            _mainWindow.ShowAlert(LanguageManager.Get("ServiceUpdated"));
            Close();
        };
    }

    private void SetupWorker(Worker worker)
    {
        Title = LanguageManager.Get("UpdateWorker");

        var nameLabel = new TextBlock
        {
            Text = LanguageManager.Get("WorkerName"),
            FontWeight = FontWeight.Bold
        };
        var nameBox = new TextBox { Width = 350, Text = worker.Name };

        var passwordLabel = new TextBlock
        {
            Text = LanguageManager.Get("WorkerPassword"),
            FontWeight = FontWeight.Bold
        };
        var passwordBox = new TextBox { Width = 350, Text = worker.Password };

        var roleLabel = new TextBlock
        {
            Text = LanguageManager.Get("WorkerRole"),
            FontWeight = FontWeight.Bold
        };
        var roleCombo = new ComboBox { Width = 350 };
        roleCombo.ItemsSource = _db.Roles.Select(r => r.Name).ToList();
        roleCombo.SelectedItem = worker.Role;

        ContentPanel.Children.Add(nameLabel);
        ContentPanel.Children.Add(nameBox);
        ContentPanel.Children.Add(passwordLabel);
        ContentPanel.Children.Add(passwordBox);
        ContentPanel.Children.Add(roleLabel);
        ContentPanel.Children.Add(roleCombo);

        _onSave = () =>
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Text) ||
                roleCombo.SelectedItem == null)
            {
                _mainWindow.ShowAlert(LanguageManager.Get("FillAllFields"));
                return;
            }

            if (nameBox.Text.Trim().Length < 3)
            {
                _mainWindow.ShowAlert("Worker name must be at least 3 characters long!");
                return;
            }

            if (passwordBox.Text.Trim().Length < 3)
            {
                _mainWindow.ShowAlert("Password must be at least 3 characters long!");
                return;
            }

            if (worker.Name == "admin")
            {
                _mainWindow.ShowAlert(LanguageManager.Get("CannotModifyDefault"));
                return;
            }

            worker.Name = nameBox.Text.Trim();
            worker.Password = passwordBox.Text.Trim();
            worker.Role = roleCombo.SelectedItem.ToString()!;
            _db.SaveChanges();
            _mainWindow.ShowAlert(LanguageManager.Get("WorkerUpdated"));
            Close();
        };
    }

    private void SetupStatus(CarService log)
    {
        Title = LanguageManager.Get("ChangeStatus");
        Width = 300;
        Height = 200;

        var statusLabel = new TextBlock
        {
            Text = LanguageManager.Get("Status"),
            FontWeight = FontWeight.Bold
        };
        var statusCombo = new ComboBox { Width = 250 };
        statusCombo.Items.Add("Alustamata");
        statusCombo.Items.Add("Pooleli");
        statusCombo.Items.Add("Valmis");
        statusCombo.SelectedItem = log.Status;

        ContentPanel.Children.Add(statusLabel);
        ContentPanel.Children.Add(statusCombo);

        _onSave = () =>
        {
            log.Status = statusCombo.SelectedItem?.ToString() ?? "Alustamata";
            _db.SaveChanges();
            Close();
        };
    }

    private void SetupWorkerAssignment(CarService log)
    {
        Title = LanguageManager.Get("AssignWorker");
        Width = 400;
        Height = 200;

        var workerLabel = new TextBlock
        {
            Text = LanguageManager.Get("WorkerName"),
            FontWeight = FontWeight.Bold
        };
        var workerCombo = new ComboBox { Width = 350 };
        var workers = new List<Worker> { new Worker { Id = 0, Name = "Määramata", Password = "", Role = "" } };
        workers.AddRange(_db.Workers.ToList());
        workerCombo.ItemsSource = workers;
        workerCombo.ItemTemplate = new FuncDataTemplate<Worker>((w, _) => new TextBlock { Text = w?.Name });
        workerCombo.SelectedItem = log.Worker ?? workers[0];

        ContentPanel.Children.Add(workerLabel);
        ContentPanel.Children.Add(workerCombo);

        _onSave = () =>
        {
            var selectedWorker = workerCombo.SelectedItem as Worker;
            log.WorkerId = selectedWorker?.Id == 0 ? null : selectedWorker?.Id;
            _db.SaveChanges();
            Close();
        };
    }

    private void SaveBtn_Click(object? sender, RoutedEventArgs e)
    {
        _onSave?.Invoke();
    }

    private void CancelBtn_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}