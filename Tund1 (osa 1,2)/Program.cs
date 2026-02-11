namespace Tund1
{
    internal class StartClass
    {

        public static void Main1(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            int a = 1000;
            Console.WriteLine("Tere tulemast!");
            string tekst = Console.ReadLine();
            Console.WriteLine($"Teie sisestatud tekst on: {tekst}");
            Console.Write($"Esimene arv on {a}, siseta b: ");
            int b = int.Parse(Console.ReadLine());
            Console.WriteLine("Esimene arv on {0}, sisesta b: {1}, summa: {2}", a, b, a + b);
            Console.WriteLine("Ujukomaarvud");
            double d = double.Parse(Console.ReadLine());
            Console.WriteLine(d);
            float f = float.Parse(Console.ReadLine());
            Console.WriteLine(f);

            Random rnd = new Random();
            a = rnd.Next(1, 11);
            Console.WriteLine(a);

            float vastus = osa2.Kalkulaator(5, 2);
            Console.WriteLine($"Kalkulaatori vastus: {vastus}");
        }
    }
}