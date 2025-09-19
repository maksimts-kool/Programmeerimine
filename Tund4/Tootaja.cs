using System;

namespace Tund4;

public class Tootaja : Inimene
{
    public string Ametikoht = "Keevitaja";
    public double Tunnitasu = 15.50;
    public int Tunnid { get; set; }
    public void Tootan()
    {
        Console.WriteLine($"{Nimi} töötab {Ametikoht}");
    }
    /*public override void midaTeeb()
    {
        Console.WriteLine($"{Nimi} töötab ametikohal");
    }*/
    public double ArvutaPalk()
    {
        return Tunnitasu * Tunnid;
    }
}
