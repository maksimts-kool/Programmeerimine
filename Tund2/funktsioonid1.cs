using System.Runtime.Serialization;

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
    public static string KuniMarksonani(string märksõna)
    {
        string fraas = "";
        do
        {
            Console.WriteLine("Arva ära");
            fraas = Console.ReadLine();
        } while (fraas.ToLower() != märksõna.ToLower());
        return fraas;
    }
    public static string ArvaArv()
    {
        int oigenumber = Random.Shared.Next(1, 100);
        int maxkatseid = 5;
        for (int i = 0; i < maxkatseid; i++)
        {
            Console.WriteLine("Arva ära arv 1-100");
            int arv = int.Parse(Console.ReadLine());
            if (arv < oigenumber)
            {
                Console.WriteLine("Arv on liiga väike");
            }
            else if (arv > oigenumber)
            {
                Console.WriteLine("Arv on liiga suur");
            }
            else
            {
                return "Õige";
            }
        }
        return $"Õige number oli {oigenumber}";
    }
    public static int SuurimNeljarv(int[] arvud)
    {
        Array.Sort(arvud);         // [3,4,1,2] -> [1,2,3,4]
        Array.Reverse(arvud);      // [1,2,3,4] -> [4,3,2,1]

        int tulemus = 0;
        for (int i = 0; i < 4; i++)
        {
            tulemus = tulemus * 10 + arvud[i]; // [4,3,2,1] -> 4321
        }
        return tulemus;
    }
    public static int[,] GenereeriKorrutustabel(int ridadeArv, int veergudeArv)
    {
        int[,] tabel = new int[ridadeArv, veergudeArv];
        for (int i = 1; i <= ridadeArv; i++)
        {
            for (int j = 1; j <= veergudeArv; j++)
            {
                tabel[i - 1, j - 1] = i * j; // massiivid algavad 0
                Console.Write(string.Format("{0,4}", tabel[i - 1, j - 1]));
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        return tabel;
    }
}
