using System;

namespace Tund4.Soidukid;

public class Auto : ISoiduk
{
    // kütusekulu ja teepikkus
    public double Kütusekulu { get; set; }
    public double Teepikkus { get; set; }
    public Auto(double kütusekulu, double teepikkus)
    {
        Kütusekulu = kütusekulu;
        Teepikkus = teepikkus;
    }
    public double ArvutaKulu() => (Kütusekulu / 100) * Teepikkus; // l/100km * km = l
    public double ArvutaVahemaa() => Teepikkus;
    public override string ToString()
    {
        return $"Auto: kütusekulu {Kütusekulu} l/100km, teepikkus {ArvutaVahemaa()} km, " +
               $"kulu {ArvutaKulu():F2} l";
    }
}
