namespace Tund2;

public class funktsioonid1
{
    public static int[] GenereeriRuudud(int min, int max)
    {
        int m = Random.Shared.Next(min, max);
        int n = Random.Shared.Next(min, max);
        int[] ruudud = new int[Math.Abs(m - n) + 1];
        int k = 0;
        if (m < n)
        {
            for (int i = m; i <= n; i++)
            {
                ruudud[k] = i * i;
                k++;
            }
        }
        else
        {
            for (int i = n; i <= m; i++)
            {
                ruudud[k] = i * i;
                k++;
            }
        }
        return ruudud;
    }
    public static double[] TekstisArvud(string tekst)
    {
        Console.WriteLine("Sisesta arvud, eraldatud tühikuga:");
        string sisend = Console.ReadLine();
        string[] osad = sisend.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double[] arvud = new double[osad.Length];
        for (int i = 0; i < osad.Length; i++)
        {
            arvud[i] = double.Parse(osad[i]);
        }
        return arvud;
    }
    public static Tuple<double, double, double> AnalüüsiArve(double[] arvud)
    {
        double summa = arvud.Sum();
        double keskmine = arvud.Average();
        double korrutis = 1;
        foreach (double arv in arvud)
        {
            korrutis *= arv;
        }
        return Tuple.Create(summa, keskmine, korrutis);
    }
    public static Tuple<int, double, Inimene, Inimene> Statistika(List<Inimene> inimesed)
    {
        int summa = inimesed.Sum(x => x.Vanus);
        double keskmine = inimesed.Average(x => x.Vanus);
        Inimene vanim = inimesed[0];
        Inimene noorim = inimesed[0];
        foreach (Inimene inimene in inimesed)
        {
            if (inimene.Vanus > vanim.Vanus)
            {
                vanim = inimene;
            }
            if (inimene.Vanus < noorim.Vanus)
            {
                noorim = inimene;
            }
        }
        return Tuple.Create(summa, keskmine, noorim, vanim);
    }
}
