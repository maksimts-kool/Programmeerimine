namespace Tund4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Inimene inimene1 = new Inimene();
            inimene1.Nimi = "Juku";
            inimene1.Vanus = 12;
            inimene1.Tervita();

            Inimene inimene2 = new Inimene("Kati", 28);
            inimene2.Tervita();

            Tootaja tootaja1 = new Tootaja();
            tootaja1.Nimi = "Mati";
            tootaja1.Vanus = 45;
            tootaja1.Ametikoht = "Autojuht";
            tootaja1.Tunnid = 160;
            tootaja1.Tervita();
            tootaja1.Tootan();

            double palk = tootaja1.ArvutaPalk();
            Console.WriteLine($"Algne konto on {tootaja1.Konto.Saldo} eurot.");
            tootaja1.Konto.Saldo += palk;
            Console.WriteLine($"Uus palk, ja nüüd on kontol {tootaja1.Konto.Saldo} eurot.");
            tootaja1.Konto.VotaRaha(200);
            Console.WriteLine($"Sularaha võtmine, ja nüüd on kontol {tootaja1.Konto.Saldo} eurot.");
            tootaja1.Konto.LisaRaha(500);
            Console.WriteLine($"Lisa raha, ja nüüd on kontol {tootaja1.Konto.Saldo} eurot.");

            Dokumendid dokument1 = new Dokumendid();
            dokument1.Nimi = "Eest";
            dokument1.Vanus = 23;
            dokument1.meditsiiniraamat = true;
            dokument1.õigustööle = true;
            dokument1.Tervita();
            dokument1.Kontrolli();

            Kass loom1 = new Kass();
            loom1.Nimi = "Huss";
            loom1.Vanus = 4;
            loom1.Tyyp = "Kass";
            loom1.Toit = "Vili";
            loom1.Varv = "Pruun";
            loom1.Tervita();
            loom1.Kirjeldus();

            Karu loom2 = new Karu();
            loom2.Nimi = "Huss2";
            loom2.Vanus = 12;
            loom2.Tyyp = "Karu";
            loom2.Tervita();
            loom2.Info();


            List<IKujund> kujundid = new List<IKujund>();
            while (true)
            {
                Console.WriteLine("\nVali kujund: 1=Ruut, 2=Ring, 3=Kolmnurk, 0=Välju");
                string valik = Console.ReadLine() ?? "";

                if (valik == "0") break;

                switch (valik)
                {
                    case "1":
                        Console.Write("Sisesta küljepikkus: ");
                        double külg = double.Parse(Console.ReadLine() ?? "0");
                        kujundid.Add(new Ruut(külg));
                        break;

                    case "2":
                        Console.Write("Sisesta raadius: ");
                        double r = double.Parse(Console.ReadLine() ?? "0");
                        kujundid.Add(new Ring(r));
                        break;

                    case "3":
                        Console.Write("Sisesta kolm külge (A B C): ");
                        string[] osad = (Console.ReadLine() ?? "").Split();
                        if (osad.Length >= 3)
                        {
                            double a = double.Parse(osad[0]);
                            double b = double.Parse(osad[1]);
                            double c = double.Parse(osad[2]);
                            kujundid.Add(new Kolmnurk(a, b, c));
                        }
                        break;

                    default:
                        Console.WriteLine("Tundmatu valik.");
                        break;
                }
            }
            Console.WriteLine("\n--- Kujundite tulemused ---");
            foreach (var kujund in kujundid)
            {
                Console.WriteLine($"Pindala: {kujund.ArvutaPindala():F2}, Ümbermõõt: {kujund.ArvutaÜmbermõõt():F2}");
            }



            List<ISoiduk> soidukid = new List<ISoiduk>();
            try
            {
                string[] readLines = File.ReadAllLines(@"..\..\..\Soidukid\soidukid.txt");
                foreach (var l in readLines)
                {
                    string[] osad = l.Split(',');
                    switch (osad[0].ToLower())
                    {
                        case "auto":
                            try
                            {
                                double aKulu = double.Parse(osad[1]); // kütusekulu
                                double aKm = double.Parse(osad[2]); // teepikkus
                                soidukid.Add(new Soidukid.Auto(aKulu, aKm)); // lisame uus transporti
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Auto andmete lugemine viga.");
                            }
                            break;

                        case "jalgratas":
                            try
                            {
                                double jKm = double.Parse(osad[1]); // teepikkus
                                soidukid.Add(new Soidukid.Jalgratas(jKm));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Jalgratta andmete lugemine viga.");
                            }
                            break;

                        case "buss":
                            try
                            {
                                double bKulu = double.Parse(osad[1]);
                                double bKm = double.Parse(osad[2]);
                                int reisijad = int.Parse(osad[3]); // reisijate arv
                                soidukid.Add(new Soidukid.Buss(bKulu, bKm, reisijad));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Bussi andmete lugemine viga.");
                            }
                            break;

                        case "mootorratas":
                            try
                            {
                                double mKulu = double.Parse(osad[1]);
                                double mKm = double.Parse(osad[2]);
                                soidukid.Add(new Soidukid.Mootorratas(mKulu, mKm));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Mootorratta andmete lugemine viga.");
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Faili viga: " + e.Message);
            }
            Console.WriteLine("\n--- Sõidukite tulemused ---");
            double totalKulu = 0;
            foreach (var s in soidukid) // iga sõiduk
            {
                Console.WriteLine(s); // toString()
                totalKulu += s.ArvutaKulu(); // kogu kulu
            }
            Console.WriteLine($"\nKokku kõikide sõidukite kulu: {totalKulu:F2} l");
        }
    }
}
