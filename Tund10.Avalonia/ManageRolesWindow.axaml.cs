using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System.Linq;
using Tund10.Avalonia.Models;

namespace Tund10.Avalonia;

public partial class ManageRolesWindow : Window
{
    private readonly AutoDbContext _db;
    private readonly MainWindow _mainWindow;

    public ManageRolesWindow(AutoDbContext db, MainWindow mainWindow)
    {
        InitializeComponent();
        _db = db;
        _mainWindow = mainWindow;

        AddRoleBtn.Content = LanguageManager.Get("AddRole");
        SaveBtn.Content = LanguageManager.Get("Save");
        CloseBtn.Content = LanguageManager.Get("Cancel");

        LoadRoles();
    }

    private void LoadRoles()
    {
        _db.ChangeTracker.Clear();
        RolesGrid.LoadingRow -= RolesGrid_LoadingRow;
        RolesGrid.ItemsSource = _db.Roles.ToList();
        RolesGrid.LoadingRow += RolesGrid_LoadingRow;
        UpdateTableHeaders();
        SetupActionColumn();
    }

    private void UpdateTableHeaders()
    {
        ((DataGridTextColumn)RolesGrid.Columns[0]).Header =
            LanguageManager.Get("WorkerRole");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[1]).Header =
            LanguageManager.Get("ViewOwners");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[2]).Header =
            LanguageManager.Get("ManageOwners");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[3]).Header =
            LanguageManager.Get("ViewCars");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[4]).Header =
            LanguageManager.Get("ManageCars");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[5]).Header =
            LanguageManager.Get("ViewServices");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[6]).Header =
            LanguageManager.Get("ManageServices");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[7]).Header =
            LanguageManager.Get("CanChangeStatus");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[8]).Header =
            LanguageManager.Get("ViewWorkers");
        ((DataGridCheckBoxColumn)RolesGrid.Columns[9]).Header =
            LanguageManager.Get("ManageWorkers");
        ((DataGridTemplateColumn)RolesGrid.Columns[10]).Header =
            LanguageManager.Get("Actions");
    }

    private void RolesGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (e.Row.DataContext is Role role &&
            (role.Name == "Owner" || role.Name == "Viewer"))
        {
            e.Row.IsEnabled = false;
        }
        else if (e.Row.DataContext is Role)
        {
            e.Row.IsEnabled = true;
        }
    }

    private void SetupActionColumn()
    {
        var actionsCol = (DataGridTemplateColumn)RolesGrid.Columns[10];
        actionsCol.CellTemplate = new FuncDataTemplate<Role>((role, _) =>
        {
            var deleteBtn = new Button
            {
                Content = LanguageManager.Get("DeleteBtn"),
                Width = 80,
                Classes = { "danger" }
            };
            deleteBtn.Click += (s, e) => DeleteRole_Click(role);
            return deleteBtn;
        });
    }

    private async void AddRoleBtn_Click(object? sender, RoutedEventArgs e)
    {
        var dlg = new RoleEditWindow(_db, null);
        await dlg.ShowDialog(this);
        LoadRoles();
        _mainWindow.LoadRoles();
    }

    private void SaveBtn_Click(object? sender, RoutedEventArgs e)
    {
        // Validate all roles before saving
        foreach (var role in _db.Roles)
        {
            // Check if role has any permission
            bool hasAnyPermission = role.CanViewOwners ||
                                   role.CanManageOwners ||
                                   role.CanViewCars ||
                                   role.CanManageCars ||
                                   role.CanViewServices ||
                                   role.CanManageServices ||
                                   role.CanChangeStatus ||
                                   role.CanViewWorkers ||
                                   role.CanManageWorkers;

            if (!hasAnyPermission)
            {
                _mainWindow.ShowAlert(
                    LanguageManager.Format("RoleMustHavePermissionFormat", role.Name));
                return;
            }

            // Check if manage permissions are set without view permissions
            if ((role.CanManageOwners && !role.CanViewOwners) ||
                (role.CanManageCars && !role.CanViewCars) ||
                (role.CanManageServices && !role.CanViewServices) ||
                (role.CanManageWorkers && !role.CanViewWorkers))
            {
                _mainWindow.ShowAlert(
                    LanguageManager.Format("CannotManageWithoutViewFormat", role.Name));
                return;
            }

            // Check if change status is set without viewing cars/services
            if (role.CanChangeStatus && !role.CanViewCars && !role.CanViewServices)
            {
                _mainWindow.ShowAlert(
                    LanguageManager.Format("CannotChangeStatusWithoutViewFormat", role.Name));
                return;
            }
        }

        _db.SaveChanges();
        _mainWindow.LoadRoles();
        _mainWindow.ShowAlert(LanguageManager.Get("RolesSavedSuccessfully"));
        Close();
    }

    private void DeleteRole_Click(Role role)
    {
        if (_db.Workers.Any(w => w.Role == role.Name))
        {
            _mainWindow.ShowAlert(
                LanguageManager.Get("CannotDeleteRoleWithWorkers"));
            return;
        }

        _db.Roles.Remove(role);
        _db.SaveChanges();
        LoadRoles();
        _mainWindow.LoadRoles();
    }

    private void CloseBtn_Click(object? sender, RoutedEventArgs e)
    {
        _db.ChangeTracker.Clear();
        Close();
    }
}