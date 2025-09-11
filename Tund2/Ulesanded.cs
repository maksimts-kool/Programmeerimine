using System;

namespace Tund2;

public class Ulesanded
{
    public static void Run()
    {
        // Ül 1
        /*int[] ruudud = funktsioonid1.GenereeriRuudud(-10, 10);
        foreach (int ruut in ruudud)
        {
            Console.WriteLine(ruut);
        }

        // Ül 2
        double[] arvud = funktsioonid1.TekstisArvud("Sisesta arvud, eraldatud tühikuga:");
        var tulemus = funktsioonid1.AnalüüsiArve(arvud);
        Console.WriteLine($"Summa: {tulemus.Item1:f2}, Keskmine: {tulemus.Item2:f2}, Korrutis: {tulemus.Item3:f2}");

        // Ül 3
        List<Inimene> inimesed = new List<Inimene>();
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{i + 1}. Inimene:");
            Console.Write("Nimi: ");
            string nimi = Console.ReadLine();
            Console.Write("Vanus: ");
            int vanus = int.Parse(Console.ReadLine());
            inimesed.Add(new Inimene(nimi, vanus));
        }
        var stat = funktsioonid1.Statistika(inimesed);
        Console.WriteLine($"Vanuse summa: {stat.Item1}, Keskmine vanus: {stat.Item2:f2}");
        Console.WriteLine("Noorim inimene:");
        stat.Item3.PrindiAndmed();
        Console.WriteLine("Vanem inimene:");
        stat.Item4.PrindiAndmed();

        // Ül 4
        Console.WriteLine("Sisesta märksõna:");
        string märksõna = Console.ReadLine();
        string tulemus2 = funktsioonid1.KuniMarksonani(märksõna);
        Console.WriteLine(tulemus2);

        // Ül 5
        while (true)
        {
            string tulemus4 = funktsioonid1.ArvaArv();
            Console.WriteLine(tulemus4);

            Console.WriteLine("Kas tahad veel mängida? (jah/ei)");
            string vastus = Console.ReadLine();

            if (vastus != "jah")
            {
                Console.WriteLine("Head aega!");
                break;
            }
        }*/

        // Ül 6
        int[] arvud = new int[4];

        for (int i = 0; i < 4; i++)
        {
            while (true)
            {
                Console.Write($"Sisesta {i + 1}. number: ");
                string sisend = Console.ReadLine();
                int number = int.Parse(sisend);

                if (number < 0 || number > 9)
                {
                    Console.WriteLine("Viga!");
                }
                else
                {
                    arvud[i] = number;
                    break;
                }
            }
        }
        int suurimNeljarv = funktsioonid1.SuurimNeljarv(arvud);
        Console.WriteLine($"Suurim neljakohaline arv: {suurimNeljarv}");

        // Ül 7
        int[,] tabel = funktsioonid1.GenereeriKorrutustabel(10, 10);
        Console.WriteLine("Sisesta esimene arv:");
        int a = int.Parse(Console.ReadLine());
        Console.WriteLine("Sisesta teine arv:");
        int b = int.Parse(Console.ReadLine());
        int tulemus = tabel[a - 1, b - 1]; // sest algab 0
        Console.WriteLine($"Mis arv on {a} x {b}? Vastus: {tulemus}");
    }
}
