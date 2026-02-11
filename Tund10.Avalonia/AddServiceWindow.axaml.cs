using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Linq;
using Tund10.Avalonia.Models;

namespace Tund10.Avalonia;

public partial class AddServiceWindow : Window
{
    private readonly AutoDbContext _db;
    private readonly int _carId;
    private readonly MainWindow _mainWindow;

    public AddServiceWindow()
    {
        InitializeComponent();
        _db = null!;
        _carId = 0;
        _mainWindow = null!;
    }

    public AddServiceWindow(AutoDbContext db, int carId, Car car, MainWindow mainWindow)
    {
        InitializeComponent();
        _db = db;
        _carId = carId;
        _mainWindow = mainWindow;

        Title = LanguageManager.Get("AddServiceTitle");
        CarInfoText.Text = $"{LanguageManager.Get("Car")}: {car.Brand} {car.Model} ({car.RegistrationNumber})";
        ServiceCombo.ItemsSource = _db.Services.ToList();
        ServiceCombo.ItemTemplate = new FuncDataTemplate<Service>((s, _) => new TextBlock { Text = s?.Name });
        DatePicker.SelectedDate = DateTime.Now;
        HourCombo.ItemsSource = Enumerable.Range(0, 24).Select(h => $"{h:D2}:00").ToList();
        HourCombo.SelectedIndex = DateTime.Now.Hour;
        SaveBtn.Content = LanguageManager.Get("Save");
        CancelBtn.Content = LanguageManager.Get("Cancel");
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        ServiceLabel.Text = LanguageManager.Get("Service");
        DateLabel.Text = LanguageManager.Get("Date");
        HourLabel.Text = LanguageManager.Get("Hour");
        MileageLabel.Text = LanguageManager.Get("Mileage");
    }

    private void SaveBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (ServiceCombo.SelectedItem is not Service service ||
            !DatePicker.SelectedDate.HasValue ||
            HourCombo.SelectedIndex == -1 ||
            string.IsNullOrWhiteSpace(MileageBox.Text))
        {
            _mainWindow.ShowAlert(LanguageManager.Get("FillAllFields"));
            return;
        }

        if (!int.TryParse(MileageBox.Text, out int mileage) || mileage <= 0)
        {
            _mainWindow.ShowAlert(LanguageManager.Get("InvalidMileage"));
            return;
        }

        DateTime dateTime = DatePicker.SelectedDate.Value.Date.AddHours(HourCombo.SelectedIndex);
        if (_db.CarServices.Any(cs => cs.DateOfService == dateTime))
        {
            _mainWindow.ShowAlert(LanguageManager.Get("LogExists"));
            return;
        }

        _db.CarServices.Add(new CarService
        {
            CarId = _carId,
            ServiceId = service.Id,
            Mileage = mileage,
            DateOfService = dateTime
        });
        _db.SaveChanges();
        _mainWindow.ShowAlert(LanguageManager.Get("ServiceAdded"));
        Close();
    }

    private void CancelBtn_Click(object? sender, RoutedEventArgs e) => Close();
}