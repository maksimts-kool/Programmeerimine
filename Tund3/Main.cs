namespace Tund3;

internal class StartClass
{
    public static void Main(string[] args)
    {
        if (!Funktsioonid.SetPath())
            return;

        /*Funktsioonid.Failikirjutamine();
        Funktsioonid.Faililugemine();
        Funktsioonid.Ridadelugemine();*/

        // Ülesanded
        // Ül 1
        var tooted = Funktsioonid.LoeTootedFailist();

        var inimene = Funktsioonid.AndemedInimene();
        double vajadus = inimene.Energiavajadus();

        Console.WriteLine($"\nPäevane energiavajadus: {vajadus:f0} kcal");
        Console.WriteLine("Kui palju võiks süüa:\n");

        foreach (var toode in tooted)
        {
            if (toode.Kalorid100g > 0)
            {
                double kogusGrammi = vajadus / toode.Kalorid100g * 100;
                Console.WriteLine($"{toode.Nimi}: {kogusGrammi:f0}g päevas");
            }
        }

        // Ül 2
        var maakonnad = Funktsioonid.LoeMaakonnadFailist();
        while (true)
        {
            Console.WriteLine("\n--- MENÜÜ ---");
            Console.WriteLine("1. Leia maakond pealinna järgi");
            Console.WriteLine("2. Leia pealinn maakonna järgi");
            Console.WriteLine("3. Alusta mängu");
            Console.WriteLine("0. Välju");
            Console.Write("Valik: ");
            int valik = int.Parse(Console.ReadLine());

            switch (valik)
            {
                case 1:
                    Funktsioonid.OtsiMaakond(maakonnad);
                    break;
                case 2:
                    Funktsioonid.OtsiPealinn(maakonnad);
                    break;
                case 3:
                    Funktsioonid.ManguReziim(maakonnad);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Tundmatu valik!");
                    break;
            }
        }
    }
}