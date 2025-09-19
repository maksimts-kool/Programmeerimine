using System;

namespace Tund4;

public class Kass : Loom
{
    public string Toit;
    public string Varv;
    public void Kirjeldus()
    {
        Console.WriteLine($"{Nimi} on {Varv} varvi, ta sööb {Toit}");
    }
}
