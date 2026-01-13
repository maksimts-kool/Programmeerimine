using System;
using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Ankeet
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nimi on kohustuslik")]
    [StringLength(200, ErrorMessage = "Nimi on liiga pikk")]
    public string Nimi { get; set; } = string.Empty;

    [Display(Name = "Loomise kuupäev")]
    [DataType(DataType.Date)]
    public DateTime LoomiseKupaev { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "E-post on kohustuslik")]
    [EmailAddress(ErrorMessage = "E-post ei ole korrektne")]
    [StringLength(200, ErrorMessage = "E-post on liiga pikk")]
    [Display(Name = "E-post")]
    public string Epost { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Vali üritus")]
    [Display(Name = "Üritus")]
    public int PyhaId { get; set; }

    public Pyha? Pyha { get; set; }

    [Display(Name = "Kes osaleb")]
    public bool OnOsaleb { get; set; } = true;
}
