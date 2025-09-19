using System;

namespace Tund4;

public class Loom
{
    public string Nimi;
    public string Tyyp;
    public int Vanus;
    public Loom() { }
    public Loom(string nimi, string tyyp, int vanus)
    {
        Nimi = nimi;
        Tyyp = tyyp;
        Vanus = vanus;
    }
    public void Tervita()
    {
        Console.WriteLine($"See on {Tyyp}, tema nimi on {Nimi} ja ta on {Vanus} aastat vana");
    }
}
