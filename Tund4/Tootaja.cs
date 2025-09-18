using System;

namespace Tund4;

public class Tootaja : Inimene
{
    public string Ametikoht = "Keevitaja";
    public void Tootan()
    {
        Console.WriteLine($"{Nimi} töötab {Ametikoht}");
    }
}
