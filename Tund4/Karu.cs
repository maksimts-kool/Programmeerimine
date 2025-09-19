using System;

namespace Tund4;

public class Karu : Loom
{
    public string asukoht;
    public int pikkus;
    public void Info()
    {
        Console.WriteLine($"{Nimi} asub {asukoht}, tema pikkus on {pikkus} cm");
    }
}
