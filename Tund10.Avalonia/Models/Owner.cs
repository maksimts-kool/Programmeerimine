using System.Collections.Generic;

namespace Tund10.Avalonia.Models;

public class Owner
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Phone { get; set; }

    public List<Car> Cars { get; set; } = new();
}