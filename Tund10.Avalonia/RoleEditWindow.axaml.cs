using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using System.Linq;
using Tund10.Avalonia.Models;

namespace Tund10.Avalonia;

public partial class RoleEditWindow : Window
{
    private readonly AutoDbContext _db;
    private readonly Role? _role;

    public RoleEditWindow()
    {
        InitializeComponent();
        _db = null!;
    }

    public RoleEditWindow(AutoDbContext db, Role? role)
    {
        InitializeComponent();
        _db = db;
        _role = role;

        UpdateLanguage();

        if (role != null)
        {
            Title = LanguageManager.Get("UpdateWorker");
            RoleNameBox.Text = role.Name;
            ViewOwnersCheck.IsChecked = role.CanViewOwners;
            OwnersCheck.IsChecked = role.CanManageOwners;
            ViewCarsCheck.IsChecked = role.CanViewCars;
            CarsCheck.IsChecked = role.CanManageCars;
            ViewServicesCheck.IsChecked = role.CanViewServices;
            ServicesCheck.IsChecked = role.CanManageServices;
            StatusCheck.IsChecked = role.CanChangeStatus;
            ViewWorkersCheck.IsChecked = role.CanViewWorkers;
            WorkersCheck.IsChecked = role.CanManageWorkers;
        }
        else
        {
            Title = LanguageManager.Get("AddRole");
        }
    }

    private void UpdateLanguage()
    {
        RoleNameBox.Watermark = LanguageManager.Get("WorkerRole");
        OwnersLabel.Text = LanguageManager.Get("Owners");
        ViewOwnersCheck.Content = LanguageManager.Get("ViewOwners");
        OwnersCheck.Content = LanguageManager.Get("ManageOwners");
        CarsLabel.Text = LanguageManager.Get("Cars");
        ViewCarsCheck.Content = LanguageManager.Get("ViewCars");
        CarsCheck.Content = LanguageManager.Get("ManageCars");
        ServicesLabel.Text = LanguageManager.Get("ServicesLogs");
        ViewServicesCheck.Content = LanguageManager.Get("ViewServices");
        ServicesCheck.Content = LanguageManager.Get("ManageServices");
        StatusCheck.Content = LanguageManager.Get("CanChangeStatus");
        WorkersLabel.Text = LanguageManager.Get("Workers");
        ViewWorkersCheck.Content = LanguageManager.Get("ViewWorkers");
        WorkersCheck.Content = LanguageManager.Get("ManageWorkers");
        SaveBtn.Content = LanguageManager.Get("Save");
        CancelBtn.Content = LanguageManager.Get("Cancel");
    }

    private void SaveBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RoleNameBox.Text))
        {
            ShowAlert(LanguageManager.Get("RoleNameRequired"));
            return;
        }

        string roleName = RoleNameBox.Text.Trim();

        if (roleName.Length < 3)
        {
            ShowAlert(LanguageManager.Get("RoleNameMinLength"));
            return;
        }

        // Check if role name already exists (excluding current role if editing)
        bool roleExists = _db.Roles.Any(r =>
            r.Name.ToLower() == roleName.ToLower() &&
            (_role == null || r.Id != _role.Id));

        if (roleExists)
        {
            ShowAlert(LanguageManager.Get("RoleNameExists"));
            return;
        }

        bool hasAnyPermission = ViewOwnersCheck.IsChecked == true ||
                               OwnersCheck.IsChecked == true ||
                               ViewCarsCheck.IsChecked == true ||
                               CarsCheck.IsChecked == true ||
                               ViewServicesCheck.IsChecked == true ||
                               ServicesCheck.IsChecked == true ||
                               StatusCheck.IsChecked == true ||
                               ViewWorkersCheck.IsChecked == true ||
                               WorkersCheck.IsChecked == true;

        if (!hasAnyPermission)
        {
            ShowAlert(LanguageManager.Get("RoleMustHavePermission"));
            return;
        }

        // Check if manage permissions are set without view permissions
        if ((OwnersCheck.IsChecked == true && ViewOwnersCheck.IsChecked != true) ||
            (CarsCheck.IsChecked == true && ViewCarsCheck.IsChecked != true) ||
            (ServicesCheck.IsChecked == true && ViewServicesCheck.IsChecked != true) ||
            (WorkersCheck.IsChecked == true && ViewWorkersCheck.IsChecked != true))
        {
            ShowAlert(LanguageManager.Get("CannotManageWithoutView"));
            return;
        }

        // Check if change status is set without viewing cars/services
        if (StatusCheck.IsChecked == true &&
            ViewCarsCheck.IsChecked != true &&
            ViewServicesCheck.IsChecked != true)
        {
            ShowAlert(LanguageManager.Get("CannotChangeStatusWithoutView"));
            return;
        }

        if (_role != null)
        {
            _role.Name = roleName;
            _role.CanViewOwners = ViewOwnersCheck.IsChecked ?? false;
            _role.CanManageOwners = OwnersCheck.IsChecked ?? false;
            _role.CanViewCars = ViewCarsCheck.IsChecked ?? false;
            _role.CanManageCars = CarsCheck.IsChecked ?? false;
            _role.CanViewServices = ViewServicesCheck.IsChecked ?? false;
            _role.CanManageServices = ServicesCheck.IsChecked ?? false;
            _role.CanChangeStatus = StatusCheck.IsChecked ?? false;
            _role.CanViewWorkers = ViewWorkersCheck.IsChecked ?? false;
            _role.CanManageWorkers = WorkersCheck.IsChecked ?? false;
        }
        else
        {
            _db.Roles.Add(new Role
            {
                Name = roleName,
                CanViewOwners = ViewOwnersCheck.IsChecked ?? false,
                CanManageOwners = OwnersCheck.IsChecked ?? false,
                CanViewCars = ViewCarsCheck.IsChecked ?? false,
                CanManageCars = CarsCheck.IsChecked ?? false,
                CanViewServices = ViewServicesCheck.IsChecked ?? false,
                CanManageServices = ServicesCheck.IsChecked ?? false,
                CanChangeStatus = StatusCheck.IsChecked ?? false,
                CanViewWorkers = ViewWorkersCheck.IsChecked ?? false,
                CanManageWorkers = WorkersCheck.IsChecked ?? false
            });
        }

        _db.SaveChanges();
        Close();
    }

    private void CancelBtn_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ShowAlert(string message)
    {
        var dlg = new Window
        {
            Width = 350,
            Height = 180,
            Title = LanguageManager.Get("Message"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
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
}