using System.ComponentModel.DataAnnotations;

namespace Tund13.Models;

public enum KursuseTase
{
    A1, A2, B1, B2, C1, C2
}

public class Keelekursus
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nimetus on kohustuslik.")]
    [Display(Name = "Kursuse nimetus")]
    public string Nimetus { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keel on kohustuslik.")]
    [Display(Name = "Keel")]
    public string Keel { get; set; } = string.Empty;

    [Display(Name = "Tase")]
    public KursuseTase Tase { get; set; }

    [Display(Name = "Kirjeldus")]
    public string? Kirjeldus { get; set; }

    // Navigatsioon
    public virtual ICollection<Koolitus> Koolitused { get; set; } = new List<Koolitus>();
}
