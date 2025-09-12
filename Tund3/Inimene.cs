using System;

namespace Tund3;

public class Inimene
{
    public string Nimi { get; set; }
    public int Vanus { get; set; }
    public string Sugu { get; set; }
    public double Pikkus { get; set; }
    public double Kaal { get; set; }
    public double Aktiivsustase { get; set; }
    public Inimene(string nimi, int vanus, string sugu, double pikkus, double kaal, double aktiivsustase)
    {
        Nimi = nimi;
        Vanus = vanus;
        Sugu = sugu;
        Pikkus = pikkus;
        Kaal = kaal;
        Aktiivsustase = aktiivsustase;
    }
    public double Energiavajadus()
        {
            double bmr;
            if (Sugu.ToLower().StartsWith("m")) // mees
            {
                bmr = 88.362 + (13.397 * Kaal) + (4.799 * Pikkus) - (5.677 * Vanus);
            }
            else // naine
            {
                bmr = 447.593 + (9.247 * Kaal) + (3.098 * Pikkus) - (4.330 * Vanus);
            }
            return bmr * Aktiivsustase;
        }
}
