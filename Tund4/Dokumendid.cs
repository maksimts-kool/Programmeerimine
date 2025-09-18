using System;

namespace Tund4;

public class Dokumendid : Inimene
{
    public bool meditsiiniraamat;
    public bool õigustööle;

    public void Kontrolli()
    {
        if (meditsiiniraamat == true && õigustööle == false)
        {
            Console.WriteLine($"{Nimi} on meditsiini raamat aga ei ole õigus tööle");
        }
        else if (meditsiiniraamat == false && õigustööle == true)
        {
            Console.WriteLine($"{Nimi} ei ole meditsiini raamat aga on õigus tööle");
        }
        else
        {
            Console.WriteLine($"{Nimi} on meditsiini raamat ja õigus tööle");
        }
    }
}
