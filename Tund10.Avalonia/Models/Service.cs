using System.Collections.Generic;

namespace Tund10.Avalonia.Models;

public class Service
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public List<CarService> CarServices { get; set; } = new();
}