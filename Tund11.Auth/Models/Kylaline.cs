using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Kylaline
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [StringLength(100, ErrorMessage = "The field {0} must be a string with a maximum length of {1}.")]
    public string Nimi { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Holiday")]
    [Required(ErrorMessage = "Validation_SelectHoliday")]
    [Range(1, int.MaxValue, ErrorMessage = "Validation_SelectHoliday")]
    public int PyhaId { get; set; }

    public Pyha? Pyha { get; set; }
}