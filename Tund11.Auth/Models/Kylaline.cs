using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Kylaline
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nimi on kohustuslik")]
    [StringLength(100, ErrorMessage = "Nimi on liiga pikk")]
    public string Nimi { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email on kohustuslik")]
    [EmailAddress(ErrorMessage = "Email ei ole korrektne")]
    [StringLength(200, ErrorMessage = "Email on liiga pikk")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Kutse")]
    public bool OnKutse { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vali püha")]
    public int PyhaId { get; set; }

    public Pyha? Pyha { get; set; }
}
