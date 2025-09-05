using System;

namespace Tund1;

public class osa2
{
    public static float Kalkulaator(float a, float b)
    {
        float vastus = a + b;
        return vastus;
    }
    public static string Kuu_nimetus(int kuunr)
    {
        string kuu = "";
        switch (kuunr)
        {
            case 1:
                kuu = "jaanuar";
                break;
            case 2:
                kuu = "veebruar";
                break;
            case 3:
                kuu = "märts";
                break;
            case 4:
                kuu = "aprill";
                break;
            case 5:
                kuu = "mai";
                break;
            case 6:
                kuu = "juuni";
                break;
            case 7:
                kuu = "juuli";
                break;
            case 8:
                kuu = "august";
                break;
            case 9:
                kuu = "september";
                break;
            case 10:
                kuu = "oktoober";
                break;
            case 11:
                kuu = "november";
                break;
            case 12:
                kuu = "detsember";
                break;
            default:
                kuu = "Vale kuu number";
                break;
        }
        return kuu;
    }
    public static string Hooaeg(int kuunr)
    {
        string hoo = "";
        if (kuunr == 1 || kuunr == 2 || kuunr == 12) //&& - and, || - or
        {
            hoo = "Talv";
        }
        else if (kuunr >= 3 && kuunr <= 5)
        {
            hoo = "Kevad";
        }
        else if (kuunr >= 6 && kuunr <= 8)
        {
            hoo = "Suvi";
        }
        else if (kuunr >= 9 && kuunr <= 11)
        {
            hoo = "Sügis";
        }
        else
        {
            hoo = "Vale kuu number";
        }
        return hoo;
    }
    public static string Piletihind(int vanus)
    {
        string hinnatyyp = "";
        if (vanus > 0 && vanus < 6)
        {
            hinnatyyp = "Tasuta";
        }
        else if (vanus >= 6 && vanus <= 14)
        {
            hinnatyyp = "Lastepilet";
        }
        else if (vanus >= 15 && vanus <= 65)
        {
            hinnatyyp = "Täispilet";
        }
        else if (vanus > 65 && vanus <= 100)
        {
            hinnatyyp = "Sooduspilet";
        }
        else
        {
            hinnatyyp = "Vale vanus";
        }
        return hinnatyyp;
    }
    public static string Pinginaaber(string nimi1, string nimi2)

    {
        return $"{nimi1} ja {nimi2} on täna pinginaabrid.";
    }
    public static string SuguPikkus(string sugu, int pikkus)
    {
        string vastus = "";
        if (sugu == "naine")
        {
            if (pikkus < 160)
            {
                vastus = "Naine - Sa oled lühikese pikkus";
            }
            else if (pikkus >= 160 && pikkus <= 175)
            {
                vastus = "Naine - Sa oled keskmise pikkus";
            }
            else if (pikkus > 175)
            {
                vastus = "Naine - Sa oled pikk pikkus";
            }
        }
        else if (sugu == "mees")
        {
            if (pikkus < 170)
            {
                vastus = "Mees - Sa oled lühikese pikkus";
            }
            else if (pikkus >= 170 && pikkus <= 185)
            {
                vastus = "Mees - Sa oled keskmise pikkus";
            }
            else if (pikkus > 185)
            {
                vastus = "Mees - Sa oled pikk pikkus";
            }
        }
        else
        {
            vastus = "Vale valik";
        }
        return vastus;
    }
    public static double Ostukorv(string tyyp, string mida)
    {
        double hind = 0;
        if (mida == "jah")
        {
            if (tyyp == "leib")
            {
                hind += 1.2;
            }
            else if (tyyp == "sai")
            {
                hind += 0.9;
            }
            else if (tyyp == "piim")
            {
                hind += 1.7;
            }
        }
        
        return hind;
    }
}
