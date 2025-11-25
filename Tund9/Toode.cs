using System;

namespace Tund9;

public class Toode
{
    public int Id { get; set; }
    public string ToodeNimetus { get; set; }
    public DateTime Lisatud { get; set; }
    public int Kogus { get; set; }
    public double Hind { get; set; }
    public string Pilt { get; set; }
    public int KategooriaId { get; set; }
    public Kategooria Kategooria { get; set; }
}

public class Kategooria
{
    public int Id { get; set; }
    public string KategooriaNimetus { get; set; }
    public string Kirjeldus { get; set; }
    public ICollection<Toode> Tooted { get; set; }
}

