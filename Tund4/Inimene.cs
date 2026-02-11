using System;

namespace Tund4;

public class Inimene
{
    public string Nimi = "";
    public int Vanus;
    public Pank Konto;
    public Inimene() { Konto = new Pank(); }
    public Inimene(string nimi, int vanus)
    {
        Nimi = nimi;
        Vanus = vanus;
        Konto = new Pank();
    }
    public void Tervita()
    {
        Console.WriteLine($"Tere {Nimi} sa oled {Vanus} aastat vana");
    }
}
