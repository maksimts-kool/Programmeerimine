namespace Tund3;

internal class Funktsioonid
{
    private static string pathh = "";
    public static bool SetPath()
    {
        try
        {
            Console.WriteLine("Sisesta faili nimi: ");
            pathh = Console.ReadLine();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", pathh);
            if (!File.Exists(path))
            {
                Console.WriteLine("Faili ei leitud!");
                return false;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Mingi viga failiga");
            return false;
        }
        return true;
    }
    public static void Failikirjutamine()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\" + pathh); //@"..\..\..\Kuud.txt"
            StreamWriter text = new StreamWriter(path, true); // true = lisa lõppu
            Console.WriteLine("Sisesta mingi tekst: ");
            string lause = Console.ReadLine();
            text.WriteLine(lause);
            text.Close();
        }
        catch (Exception)
        {
            Console.WriteLine("Mingi viga failiga");
        }
    }
    public static void FailikirjutamineV2(string sisu)
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Maakonnad.txt");
            using (StreamWriter text = new StreamWriter(path, true))
            {
                text.WriteLine(sisu);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Mingi viga failiga");
        }
    }
    public static void Faililugemine()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\" + pathh);
            StreamReader text = new StreamReader(path);
            string laused = text.ReadToEnd();
            text.Close();
            Console.WriteLine(laused);
        }
        catch (Exception)
        {
            Console.WriteLine("Mingi viga failiga, ei saa faili lugeda");
        }
    }
    public static List<string> Ridadelugemine()
    {
        List<string> kuude_list = new List<string>();
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\" + pathh);
            foreach (string rida in File.ReadAllLines(path))
            {
                kuude_list.Add(rida);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Viga failiga!");
        }
        foreach (string kuu in kuude_list)
        {
            Console.WriteLine(kuu);
        }

        // Eemalda "Juuni"
        kuude_list.Remove("Juuni");

        // Muuda esimest elementi
        if (kuude_list.Count > 0)
            kuude_list[0] = "Veeel kuuu";

        Console.WriteLine("--------------Kustutasime juuni-----------");

        foreach (string kuu in kuude_list)
        {
            Console.WriteLine(kuu);
        }

        Console.WriteLine("Sisesta kuu nimi, mida otsida:");
        string otsitav = Console.ReadLine();

        if (kuude_list.Contains(otsitav))
            Console.WriteLine("Kuu " + otsitav + " on olemas.");
        else
            Console.WriteLine("Sellist kuud pole.");
        return kuude_list;
    }

    // Ülesanded
    public static List<Toode> LoeTootedFailist()
    {
        List<Toode> tooted = new List<Toode>();
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\" + pathh);
            foreach (string rida in File.ReadAllLines(path))
            {
                string[] osad = rida.Split(',');
                string nimi = osad[0]; // Banaan
                int kalorid100g = int.Parse(osad[1]); // 89
                Toode toode = new Toode(nimi, kalorid100g);
                tooted.Add(toode);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Viga failiga!");
        }
        return tooted;
    }

    public static Inimene AndemedInimene()
    {
        Console.Write("Sisesta nimi: ");
        string nimi = Console.ReadLine();

        Console.Write("Vanus: ");
        int vanus = int.Parse(Console.ReadLine());

        Console.Write("Sugu: ");
        string sugu = Console.ReadLine();

        Console.Write("Pikkus: ");
        double pikkus = double.Parse(Console.ReadLine());

        Console.Write("Kaal: ");
        double kaal = double.Parse(Console.ReadLine());

        Console.Write("Aktiivsustase: ");
        double aktiivsustase = double.Parse(Console.ReadLine());

        return new Inimene(nimi, vanus, sugu, pikkus, kaal, aktiivsustase);
    }
    public static Dictionary<string, string> LoeMaakonnadFailist()
    {
        Dictionary<string, string> maakonnad = new Dictionary<string, string>();
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Maakonnad.txt");
            foreach (string rida in File.ReadAllLines(path))
            {
                string[] osad = rida.Split(',');
                string kood = osad[0]; // Harjumaa
                string nimi = osad[1]; // Tallinn
                maakonnad[kood] = nimi;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Viga failiga!");
        }
        return maakonnad;
    }
    public static void OtsiMaakond(Dictionary<string, string> dict)
    {
        var values = dict.Values.ToList(); // pealinnad
        var keys = dict.Keys.ToList(); // maakonnad
        Console.Write("Sisesta pealinna nimi: ");
        string linn = Console.ReadLine();
        int index = values.IndexOf(linn); 

        if (index != -1) // kui leidis
        {
            Console.WriteLine($"{linn} asub {keys[index]}");
        }
        else
        {
            Console.WriteLine("Linna ei leitud. Siis lisame.");
            Console.Write("Sisesta maakonna nimi: ");
            string maakond = Console.ReadLine();
            dict.Add(maakond, linn);
            FailikirjutamineV2($"{maakond},{linn}");
            Console.WriteLine("Lisatud");
        }
    }
    public static void OtsiPealinn(Dictionary<string, string> dict)
    {
        Console.Write("Sisesta maakonna nimi: ");
        string maakond = Console.ReadLine();

        if (dict.ContainsKey(maakond))
        {
            Console.WriteLine($"{maakond} pealinn on {dict[maakond]}");
        }
        else
        {
            Console.WriteLine("Maakonda ei leitud. Siis lisame.");
            Console.Write("Sisesta pealinna nimi: ");
            string linn = Console.ReadLine();
            dict.Add(maakond, linn);
            FailikirjutamineV2($"{maakond},{linn}");
            Console.WriteLine("Lisatud");
        }
    }
    public static void ManguReziim(Dictionary<string, string> dict)
    {
        Random rnd = new Random();
        int kokku = 0;
        int oiged = 0;

        Console.WriteLine("Mäng algas (ENTER, et lõpetada)");

        while (true)
        {
            var keys = dict.Keys.ToList(); // maakonnad
            var RKey = keys[rnd.Next(keys.Count)]; // pealinn   
            var RValue = dict[RKey]; // maakond

            if (rnd.Next(2) == 0) // 0,1 => 50/50
            {
                Console.Write($"Mis on maakonna {RKey} pealinn? ");
                string sisestus = Console.ReadLine();

                if (sisestus == "") break;

                kokku++;
                if (sisestus.ToLower() == RValue.ToLower())
                {
                    oiged++;
                    Console.WriteLine("Õige!");
                }
                else
                {
                    Console.WriteLine($"Vale! Õige vastus: {RValue}");
                }
            }
            else
            {
                Console.Write($"Millises maakonnas asub {RValue}? ");
                string sisestus = Console.ReadLine();

                if (sisestus == "") break;

                kokku++;
                if (sisestus.ToLower() == RKey.ToLower())
                {
                    oiged++;
                    Console.WriteLine("Õige!");
                }
                else
                {
                    Console.WriteLine($"Vale! Õige vastus: {RKey}");
                }
            }
        }

        if (kokku > 0)
        {
            double protsent = (double)oiged / kokku * 100; // (double) sest ei ole täisarv
            Console.WriteLine($"\nTulemus: {oiged}/{kokku} ({protsent:f0}%)");
        }
        else
        {
            Console.WriteLine("\nMäng lõpetatud.");
        }
    }
}
