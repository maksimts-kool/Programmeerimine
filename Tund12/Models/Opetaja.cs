using System.ComponentModel.DataAnnotations;

namespace Tund12.Models;

public class Opetaja
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nimi on kohustuslik.")]
    [Display(Name = "Nimi")]
    public string Nimi { get; set; } = string.Empty;

    [Display(Name = "Kvalifikatsioon")]
    public string? Kvalifikatsioon { get; set; }

    [Display(Name = "Foto")]
    public string? FotoPath { get; set; }

    // Seos sisselogimise kontoga
    [Display(Name = "Kasutajakonto")]
    public string? ApplicationUserId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    // Navigatsioon
    public virtual ICollection<Koolitus> Koolitused { get; set; } = new List<Koolitus>();
}
