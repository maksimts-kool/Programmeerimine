using System;

namespace Tund2;

public class Isik
{
    public string eesnimi = "";
    public string perenimi = "";
    public int synniaasta = 2000;
    public Isik(){}
    public Isik(string eesnimi, string perenimi)
    {
        this.eesnimi = eesnimi;
        this.perenimi = perenimi;
    }

    public void PrindiAndmed()
    {
        Console.WriteLine($"Eesnimi: {eesnimi}, Perenimi: {perenimi}, Sünniaasta: {synniaasta}");
    }
}
