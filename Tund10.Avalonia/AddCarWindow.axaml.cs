using Avalonia.Controls;
using Avalonia.Interactivity;
using Tund10.Avalonia.Models;
using System.Linq;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;

namespace Tund10.Avalonia;

public partial class AddCarWindow : Window
{
    private readonly AutoDbContext _db = new();
    private readonly int _ownerId;
    private readonly MainWindow _mainWindow;

    public AddCarWindow(int ownerId, string ownerName, MainWindow mainWindow)
    {
        InitializeComponent();
        _ownerId = ownerId;
        _mainWindow = mainWindow;

        Title = LanguageManager.Get("AddCar");
        OwnerNameText.Text = $"{LanguageManager.Get("Owner")}: {ownerName}";
        SaveBtn.Content = LanguageManager.Get("Save");
        CancelBtn.Content = LanguageManager.Get("Cancel");
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        var brandStack = (StackPanel)CarBrandBox.Parent!;
        var modelStack = (StackPanel)CarModelBox.Parent!;
        var regStack = (StackPanel)CarRegBox.Parent!;

        ((TextBlock)brandStack.Children[0]).Text = LanguageManager.Get("Brand");
        ((TextBlock)modelStack.Children[0]).Text = LanguageManager.Get("Model");
        ((TextBlock)regStack.Children[0]).Text = LanguageManager.Get("RegNumber");
    }

    private void SaveBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CarBrandBox.Text) ||
            string.IsNullOrWhiteSpace(CarModelBox.Text) ||
            string.IsNullOrWhiteSpace(CarRegBox.Text))
        {
            ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        if (CarBrandBox.Text.Trim().Length < 2)
        {
            ShowAlert("Brand must be at least 2 characters long!");
            return;
        }

        if (CarModelBox.Text.Trim().Length < 2)
        {
            ShowAlert("Model must be at least 2 characters long!");
            return;
        }

        var reg = CarRegBox.Text.Trim().ToUpper();

        if (reg.Length < 3)
        {
            ShowAlert("Registration number must be at least 3 characters long!");
            return;
        }

        var exists = _db.Cars.Any(c => c.RegistrationNumber.ToUpper() == reg);
        if (exists)
        {
            ShowAlert(LanguageManager.Get("CarExists"));
            return;
        }

        _db.Cars.Add(new Car
        {
            Brand = CarBrandBox.Text.Trim(),
            Model = CarModelBox.Text.Trim(),
            RegistrationNumber = reg,
            OwnerId = _ownerId
        });

        _db.SaveChanges();
        ShowAlert(LanguageManager.Get("CarAdded"));
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
            Width = 300,
            Height = 150,
            Title = LanguageManager.Get("Message"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 20,
            VerticalAlignment = VerticalAlignment.Center
        };

        stack.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center
        });

        var okBtn = new Button
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        okBtn.Click += (_, _) => dlg.Close();

        stack.Children.Add(okBtn);
        dlg.Content = stack;

        await dlg.ShowDialog(this);
    }
}