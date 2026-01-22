using System.ComponentModel.DataAnnotations;

namespace Tund12.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nimi { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Kvalifikatsioon { get; set; }

        [StringLength(255)]
        public string? FotoPath { get; set; }

        // Seos Identity kasutajaga
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public virtual ApplicationUser? User { get; set; }

        // Navigation properties - initsialiseeritud
        public virtual ICollection<Training> Trainings { get; set; }
            = new List<Training>();
    }
}