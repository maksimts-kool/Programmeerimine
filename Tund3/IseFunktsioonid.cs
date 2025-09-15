using System;

namespace Tund3;

internal class IseFunktsioonid
{
    // Ül 3
    public static double ArvutaKeskmineHinne(Opilane opilane)
    {
        if (!opilane.Hinded.Any())
        {
            return 0.0;
        }
        return opilane.Hinded.Average();
    }
    public static List<Opilane> SorteeriKeskmiseJargi(List<Opilane> opilased)
    {
        var sorditud = opilased.ToList();
        sorditud.Sort((a, b) => ArvutaKeskmineHinne(b).CompareTo(ArvutaKeskmineHinne(a)));
        return sorditud;
    }
    // Ül 5
    public static double[] TekstisArvud()
    {
        Console.WriteLine("Sisesta arvud, eraldatud tühikuga:");
        string sisend = Console.ReadLine();
        string[] osad = sisend.Split(',', StringSplitOptions.RemoveEmptyEntries);

        double[] arvud = new double[osad.Length];
        for (int i = 0; i < osad.Length; i++)
        {
            arvud[i] = double.Parse(osad[i]);
        }
        return arvud;
    }
    public static void AnalüüsiArvud(double[] arvud)
    {
        double max = arvud[0];
        double min = arvud[0];
        double summa = 0;

        foreach (double arv in arvud)
        {
            if (arv > max)
                max = arv;
            if (arv < min)
                min = arv;
            summa += arv;
        }

        double keskmine = summa / arvud.Length;

        int suuremadKesk = 0;
        foreach (double arv in arvud)
        {
            if (arv > keskmine)
                suuremadKesk++;
        }
        Array.Sort(arvud);
        Console.WriteLine("Sorteeritud arvud:");
        Console.WriteLine(string.Join(",", arvud));

        Console.WriteLine($"Maksimum: {max}");
        Console.WriteLine($"Miinimum: {min}");
        Console.WriteLine($"Summa: {summa}");
        Console.WriteLine($"Keskmine: {keskmine:F1}");
        Console.WriteLine($"Kui palju arve, mis on suuremad kui keskmine: {suuremadKesk}");
    }
}
