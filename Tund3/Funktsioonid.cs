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
}
