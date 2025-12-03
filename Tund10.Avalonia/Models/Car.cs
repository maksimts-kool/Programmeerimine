using System.Collections.Generic;

namespace Tund10.Avalonia.Models;

public class Car
{
    public int Id { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string RegistrationNumber { get; set; }

    public int OwnerId { get; set; }
    public Owner? Owner { get; set; }

    public List<CarService> CarServices { get; set; } = new();
}