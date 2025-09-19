using System;

namespace Tund4.Soidukid;

public class Mootorratas : ISoiduk
{
    // kütusekulu ja teepikkus
    public double Kütusekulu { get; set; }
    public double Teepikkus { get; set; }

    public Mootorratas(double kütusekulu, double teepikkus)
    {
        Kütusekulu = kütusekulu;
        Teepikkus = teepikkus;
    }

    public double ArvutaKulu() => (Kütusekulu / 100) * Teepikkus;
    public double ArvutaVahemaa() => Teepikkus;

    public override string ToString()
    {
        return $"Mootorratas: kütusekulu {Kütusekulu} l/100km, teepikkus {ArvutaVahemaa()} km, " +
                $"kulu {ArvutaKulu():F2} l";
    }
}
