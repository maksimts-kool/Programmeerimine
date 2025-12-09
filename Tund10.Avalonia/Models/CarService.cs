using System;

namespace Tund10.Avalonia.Models;

public class CarService
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car? Car { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public int? WorkerId { get; set; }
    public Worker? Worker { get; set; }

    public DateTime DateOfService { get; set; } = DateTime.Now;
    public int Mileage { get; set; }
    public string Status { get; set; } = "Alustamata";
}