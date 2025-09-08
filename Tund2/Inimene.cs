using System;

namespace Tund2;

public class Inimene
{
    public string Nimi = "";
    public int Vanus = 0;
    public Inimene(string Nimi, int Vanus)
    {
        this.Nimi = Nimi;
        this.Vanus = Vanus;
    }
    public void PrindiAndmed()
    {
        Console.WriteLine($"Nimi: {Nimi}, Vanus: {Vanus}");
    }
}
