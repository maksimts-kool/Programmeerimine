using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tund13.Models;

public class Koolitus
{
    public int Id { get; set; }

    // FK → Keelekursus
    [Required(ErrorMessage = "Keelekursus on kohustuslik.")]
    [Display(Name = "Keelekursus")]
    public int KeelekursusId { get; set; }
    public virtual Keelekursus? Keelekursus { get; set; }

    // FK → Opetaja
    [Required(ErrorMessage = "Õpetaja on kohustuslik.")]
    [Display(Name = "Õpetaja")]
    public int OpetajaId { get; set; }
    public virtual Opetaja? Opetaja { get; set; }

    [Required(ErrorMessage = "Alguskuupäev on kohustuslik.")]
    [Display(Name = "Alguskuupäev")]
    [DataType(DataType.Date)]
    public DateTime AlgusKuupaev { get; set; }

    [Required(ErrorMessage = "Lõppkuupäev on kohustuslik.")]
    [Display(Name = "Lõppkuupäev")]
    [DataType(DataType.Date)]
    public DateTime LoppKuupaev { get; set; }

    [Required(ErrorMessage = "Hind on kohustuslik.")]
    [Display(Name = "Hind (€)")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Hind { get; set; }

    [Required(ErrorMessage = "Maksimaalne osalejate arv on kohustuslik.")]
    [Display(Name = "Maks osalejaid")]
    [Range(1, 100, ErrorMessage = "Osalejate arv peab olema 1–100.")]
    public int MaxOsalejaid { get; set; }

    // Navigatsioon
    public virtual ICollection<Registreerimine> Registreerimised { get; set; } = new List<Registreerimine>();
}
