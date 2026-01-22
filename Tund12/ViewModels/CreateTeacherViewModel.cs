using System.ComponentModel.DataAnnotations;

namespace Tund12.ViewModels
{
    public class CreateTeacherViewModel
    {
        [Required(ErrorMessage = "Nimi on kohustuslik")]
        [StringLength(100)]
        public string Nimi { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-post on kohustuslik")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parool on kohustuslik")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Parool peab olema vähemalt 6 tähemärki pikk")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroolid ei ühti")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Kvalifikatsioon { get; set; }
    }
}