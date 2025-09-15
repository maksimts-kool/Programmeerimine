using System;

namespace Tund3;

public class Opilane
{
    public string Nimi { get; set; }
    public List<int> Hinded { get; set; }

    public Opilane(string nimi)
    {
        Nimi = nimi;
        Hinded = new List<int>();
    }

    public void LisaHinne(int hinne)
    {
        if (hinne < 1 || hinne > 5)
        {
            Console.WriteLine($"Vigane hinne {hinne}");
            return;
        }
        Hinded.Add(hinne);
    }
}
