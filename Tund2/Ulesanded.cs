using System;

namespace Tund2;

public class Ulesanded
{
    public static void Run()
    {
        // Ül 1
        int[] ruudud = funktsioonid1.GenereeriRuudud(-10, 10);
        foreach (int ruut in ruudud)
        {
            Console.WriteLine(ruut);
        }

        // Ül 2
        double[] arvud = funktsioonid1.TekstisArvud("Sisesta arvud, eraldatud tühikuga:");
        var tulemus = funktsioonid1.AnalüüsiArve(arvud);
        Console.WriteLine($"Summa: {tulemus.Item1:f2}, Keskmine: {tulemus.Item2:f2}, Korrutis: {tulemus.Item3:f2}");

        // Ül 3
        
    }
}
