namespace Tund3;

internal class StartClass
{
    public static void Main(string[] args)
    {
        int PROGRAMMIvalik = 2; // !! VALI PROGRAMMI !!  1,2


        if (PROGRAMMIvalik == 1)
        {
            if (!Funktsioonid.SetPath())
                return;

            Funktsioonid.Failikirjutamine();
            Funktsioonid.Faililugemine();
            Funktsioonid.Ridadelugemine();

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
                Console.WriteLine("\nMENÜÜ");
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
        else if (PROGRAMMIvalik == 2)
        {
            // Ise Töö
            // Ül 3
            var mari = new Opilane("Mari");
            mari.LisaHinne(5);
            mari.LisaHinne(4);
            mari.LisaHinne(4);

            var juhan = new Opilane("Juhan");
            juhan.LisaHinne(3);
            juhan.LisaHinne(4);
            juhan.LisaHinne(2);
            juhan.LisaHinne(2);

            var kati = new Opilane("Kati");
            kati.LisaHinne(5);
            kati.LisaHinne(5);
            kati.LisaHinne(4);
            kati.LisaHinne(4);

            var opilased = new List<Opilane> { mari, juhan, kati };
            var sort = IseFunktsioonid.SorteeriKeskmiseJargi(opilased);
            foreach (var opilanesort in sort)
            {
                Console.WriteLine(
                    $"{opilanesort.Nimi} - keskmine hind: {IseFunktsioonid.ArvutaKeskmineHinne(opilanesort):F1}"
                );
            }

            var parim = sort.First();
            Console.WriteLine(
                $"\nKõrgeim keskmine on {IseFunktsioonid.ArvutaKeskmineHinne(parim):F1}, õpilasel {parim.Nimi}"
            );

            // Ül 5
            double[] arvud = IseFunktsioonid.TekstisArvud();
            IseFunktsioonid.AnalüüsiArvud(arvud);
        }
    }
}