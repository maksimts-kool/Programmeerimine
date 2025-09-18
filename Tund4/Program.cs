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
        }
    }
}
