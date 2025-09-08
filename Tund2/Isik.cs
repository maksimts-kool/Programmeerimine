using System;

namespace Tund2;

public class Isik
{
    public string eesnimi;
    public string perenimi;
    public int synniaasta;
    public void PrindiAndmed()
    {
        Console.WriteLine($"Eesnimi: {eesnimi}, Perenimi: {perenimi}, Sünniaasta: {synniaasta}");
    }
}
