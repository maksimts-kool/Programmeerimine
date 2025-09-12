using System;

namespace Tund3;

public class Toode
{
    public string Nimi { get; set; }
    public int Kalorid100g { get; set; }
    public Toode(string nimi, int kalorid100g)
    {
        Nimi = nimi;
        Kalorid100g = kalorid100g;
    }
}
