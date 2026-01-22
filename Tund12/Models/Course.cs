using System.ComponentModel.DataAnnotations;

namespace Tund12.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nimetus on kohustuslik")]
        [StringLength(100)]
        public string Nimetus { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Keel { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Tase { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Kirjeldus { get; set; }

        // Navigation property - initsialiseeritud
        public virtual ICollection<Training> Trainings { get; set; }
            = new List<Training>();
    }
}