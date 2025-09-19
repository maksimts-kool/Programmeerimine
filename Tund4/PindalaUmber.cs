using System;

namespace Tund4;

public class Ruut : IKujund
{
    public double Külg { get; set; }

    public Ruut(double külg)
    {
        Külg = külg;
    }

    public double ArvutaPindala() => Külg * Külg;
    public double ArvutaÜmbermõõt() => 4 * Külg;
}

public class Ring : IKujund
{
    public double Raadius { get; set; }

    public Ring(double raadius)
    {
        Raadius = raadius;
    }

    public double ArvutaPindala() => Math.PI * Raadius * Raadius;
    public double ArvutaÜmbermõõt() => 2 * Math.PI * Raadius;
}

public class Kolmnurk : IKujund
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public Kolmnurk(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public double ArvutaÜmbermõõt() => A + B + C;

    public double ArvutaPindala()
    {
        double s = ArvutaÜmbermõõt() / 2;
        return Math.Sqrt(s * (s - A) * (s - B) * (s - C)); // Heroni valem
    }
}


