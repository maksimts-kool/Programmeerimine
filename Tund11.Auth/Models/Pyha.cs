using System;
using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Pyha
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nimetus on kohustuslik")]
    public string Nimetus { get; set; } = string.Empty;
    [DataType(DataType.Date, ErrorMessage = "Kuupäev on kohustuslik")]
    public DateTime Kuupaev { get; set; }

    public ICollection<Kylaline> Kylalised { get; set; } = new List<Kylaline>();
}
