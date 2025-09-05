namespace Tund1;

public class Ulesanne1
{
    internal class StartClass
    {
        public static void Main(string[] args)
        {
            // Ül 1
            Console.WriteLine("Tere, mis on sinu nimi?");
            string nimi = Console.ReadLine();
            if (nimi.ToLower() == "juku")
            {
                Console.WriteLine("Lähme sinuga kinno");
                Console.WriteLine("Kui vana sa oled?");
                int vanus = int.Parse(Console.ReadLine());
                Console.WriteLine($"Teie pileti tüüp on: {osa2.Piletihind(vanus)}");
            }
            else
            {
                Console.WriteLine("Head aega");
            }
            // Ül 2
            Console.WriteLine("Tere, siseta esimese nime");
            string vastus1 = Console.ReadLine();
            Console.WriteLine("Ssiseta teise nime");
            string vastus2 = Console.ReadLine();
            Console.WriteLine(osa2.Pinginaaber(vastus1, vastus2));

            // Ül 3
            Console.WriteLine("Tere, siseta oma seina pikkus m");
            float pikkus = float.Parse(Console.ReadLine());
            Console.WriteLine("Tere, siseta oma seina laius m");
            float laius = float.Parse(Console.ReadLine());
            Console.WriteLine($"Põranda pindala on {pikkus * laius} m2. Kas tahad teha remonti?");
            string vastus3 = Console.ReadLine();
            if (vastus3.ToLower() == "jah")
            {
                Console.WriteLine("Kui palju maksab ruutmeeter");
                float ruutmeeter = float.Parse(Console.ReadLine());
                Console.WriteLine($"Remont läheb maksma {pikkus * laius * ruutmeeter} eurot");
                return;
            }
            else
            {
                Console.WriteLine("Head aega");
            }

            // Ül 4
            Console.WriteLine("Tere, siseta alg hind");
            float hind = float.Parse(Console.ReadLine());
            Console.WriteLine($"30% soodustus on {hind - hind * 0.3} eurot");

            // Ül 5
            Console.WriteLine("Tere, milline on temperatuur?");
            int temp = int.Parse(Console.ReadLine());
            if (temp > 18)
            {
                Console.WriteLine("Teie temperatuur on rohkem kui 18 kraadi");
            }
            else
            {
                Console.WriteLine("Teie temperatuur on vähem/võrdne kui 18 kraadi");
            }

            // Ül 6
            Console.WriteLine("Siseta teie pikkus cm");
            int pikk = int.Parse(Console.ReadLine());
            if (pikk < 165)
            {
                Console.WriteLine("Sa oled lühikese pikkus");
            }
            else if (pikk >= 165 && pikk <= 185)
            {
                Console.WriteLine("Sa oled keskmise pikkus");
            }
            else if (pikk > 185)
            {
                Console.WriteLine("Sa oled pikk pikkus");
            }

            // Ül 7
            Console.WriteLine("Siseta teie sugu");
            string sugu = Console.ReadLine();
            Console.WriteLine("Siseta teie pikkus cm");
            int pikk2 = int.Parse(Console.ReadLine());
            Console.WriteLine(osa2.SuguPikkus(sugu.ToLower(), pikk2));

            // Ül 8
            Console.WriteLine("Tere, kas tahad leiba?");
            string leib = Console.ReadLine();
            double koguhind = 0;
            koguhind += osa2.Ostukorv("leib", leib.ToLower());
            Console.WriteLine("Kas tahad piima?");
            string piim = Console.ReadLine();
            koguhind += osa2.Ostukorv("piim", piim.ToLower());
            Console.WriteLine("Kas tahad saia?");
            string saia = Console.ReadLine();
            koguhind += osa2.Ostukorv("sai", saia.ToLower());
            Console.WriteLine($"Teie ostukorvi hind on {koguhind} eurot");
        }
    }
}
