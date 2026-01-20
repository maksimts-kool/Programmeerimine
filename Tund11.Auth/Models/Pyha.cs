using System;
using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Pyha
{
    public int Id { get; set; }

    [Display(Name = "Title")]
    [Required(ErrorMessage = "The {0} field is required.")]
    public string Nimetus { get; set; } = string.Empty;

    [Display(Name = "Date")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [DataType(DataType.Date)]
    public DateTime Kuupaev { get; set; }

    public ICollection<Kylaline> Kylalised { get; set; } = new List<Kylaline>();
}