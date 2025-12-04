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
        CarBrandBox.Watermark = LanguageManager.Get("Brand");
        CarModelBox.Watermark = LanguageManager.Get("Model");
        CarRegBox.Watermark = LanguageManager.Get("RegNumber");
        SaveBtn.Content = LanguageManager.Get("Save");
        CancelBtn.Content = LanguageManager.Get("Cancel");
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

        var reg = CarRegBox.Text.Trim().ToUpper();

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
