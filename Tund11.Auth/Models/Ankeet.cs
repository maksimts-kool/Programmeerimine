using System;
using System.ComponentModel.DataAnnotations;

namespace Tund11.Models;

public class Ankeet
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [StringLength(200, ErrorMessage = "The field {0} must be a string with a maximum length of {1}.")]
    public string Nimi { get; set; } = string.Empty;

    [Display(Name = "CreatedDate")]
    [DataType(DataType.Date)]
    public DateTime LoomiseKupaev { get; set; } = DateTime.Today;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
    [StringLength(200)]
    public string Epost { get; set; } = string.Empty;

    [Display(Name = "Event")]
    // Use Required for the dropdown selection
    [Required(ErrorMessage = "Validation_SelectEvent")]
    [Range(1, int.MaxValue, ErrorMessage = "Validation_SelectEvent")]
    public int PyhaId { get; set; }

    public Pyha? Pyha { get; set; }

    [Display(Name = "WhoAttends")]
    public bool OnOsaleb { get; set; } = true;
}