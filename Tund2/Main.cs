namespace Tund2;

public class Main
{
    internal class StartClass
    {
        public static void Main(string[] args)
        {
            // execute Tund2/ulesanded.cs
            Ulesanded.Run();
            return;
            // Massiviid, List, Kordused
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i + 1);
            }
            int j = 0;
            while (j < 10)
            {
                Console.WriteLine(j + 1);
                j++;
            }
            j = 0;
            do
            {
                Console.WriteLine(j + 1);
                j++;
            } while (j < 10);

            List<string> nimed = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i + 1}. Nimi: ");
                nimed.Add(Console.ReadLine());
            }
            foreach (string nimi in nimed)
            {
                Console.WriteLine(nimi);
            }

            int[] arvud = new int[10];
            j = 0;
            while (j < 10)
            {
                Console.WriteLine($"{j + 1}. Arv: ");
                arvud[j] = Random.Shared.Next(1, 101);
                j++;
            }
            foreach (int arv in arvud)
            {
                Console.WriteLine(arv);
            }

            List<Isik> isikud = new List<Isik>();
            j = 0;
            do
            {
                Console.WriteLine(j + 1);
                Isik isik = new Isik();
                Console.Write("Eesnimi: ");
                isik.eesnimi = Console.ReadLine();
                Console.Write("Perenimi: ");
                isik.perenimi = Console.ReadLine();
                isikud.Add(isik);
                j++;
            } while (j < 10);
            isikud.Sort((x, y) => x.eesnimi.CompareTo(y.eesnimi));
            Console.WriteLine($"Kokku on {isikud.Count} isikud");
            Console.WriteLine($"Kolmas on {isikud[2].eesnimi} {isikud[2].perenimi} isik");
            foreach (Isik isik in isikud)
            {
                isik.PrindiAndmed();
            }

            
        }
    }
}
