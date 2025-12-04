using System;

namespace Tund10.Avalonia.Models;

public class CarService
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car? Car { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public DateTime DateOfService { get; set; }
    public int Mileage { get; set; }
}