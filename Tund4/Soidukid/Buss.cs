using System;

namespace Tund4.Soidukid;

public class Buss : ISoiduk
{
    // kulu ja vahemaa, kulu jagatakse reisijate vahel
    public double Kütusekulu { get; set; }
    public double Teepikkus { get; set; }
    public int Reisijad { get; set; }
    public Buss(double kütusekulu, double teepikkus, int reisijad)
    {
        Kütusekulu = kütusekulu;
        Teepikkus = teepikkus;
        Reisijad = reisijad;
    }
    public double ArvutaKulu() => ((Kütusekulu / 100) * Teepikkus) / Reisijad; // l/100km * km / reisijad = l/reisija
    public double ArvutaVahemaa() => Teepikkus;
    public override string ToString()
    {
        return $"Buss: kütusekulu {Kütusekulu} l/100km, teepikkus {ArvutaVahemaa()} km, reisijad {Reisijad}, " +
               $"kulu reisija kohta {ArvutaKulu():F2} l";
    }
}
