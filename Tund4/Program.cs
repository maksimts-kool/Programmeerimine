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
            tootaja1.Tervita();
            tootaja1.Tootan();

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
        }
    }
}
