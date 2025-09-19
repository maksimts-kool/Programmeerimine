using System;

namespace Tund4.Soidukid;

public class Jalgratas : ISoiduk
{
    // ei kuluta kütust, ainult vahemaa
    public double Teepikkus { get; set; }
    public Jalgratas(double teepikkus)
    {
        Teepikkus = teepikkus;
    }
    public double ArvutaKulu() => 0; // jalgratas ei kasuta kütust
    public double ArvutaVahemaa() => Teepikkus;
    public override string ToString()
    {
        return $"Jalgratas: teepikkus {ArvutaVahemaa()} km, kulu {ArvutaKulu():F2} l";
    }
}
