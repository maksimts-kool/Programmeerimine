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
    }
}