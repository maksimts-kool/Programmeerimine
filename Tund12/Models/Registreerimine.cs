using System.ComponentModel.DataAnnotations;

namespace Tund12.Models;

public enum RegistreerimiseStaatus
{
    Ootel = 0,
    Kinnitatud = 1,
    Tuhistatud = 2
}

public class Registreerimine
{
    public int Id { get; set; }

    // FK → Koolitus
    [Required]
    [Display(Name = "Koolitus")]
    public int KoolitusId { get; set; }
    public virtual Koolitus? Koolitus { get; set; }

    // FK → Õpilase kasutajakonto (Identity)
    [Required]
    [Display(Name = "Õpilane")]
    public string ApplicationUserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    [Display(Name = "Registreerimise aeg")]
    public DateTime RegistreerimiseAeg { get; set; } = DateTime.Now;

    [Display(Name = "Staatus")]
    public RegistreerimiseStaatus Staatus { get; set; } = RegistreerimiseStaatus.Ootel;
}
