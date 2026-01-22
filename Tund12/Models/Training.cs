using System.ComponentModel.DataAnnotations;

namespace Tund12.Models
{
    public class Training
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public virtual Teacher? Teacher { get; set; }

        [Required]
        [Display(Name = "Alguskuupäev")]
        public DateTime AlgusKuupaev { get; set; }

        [Required]
        [Display(Name = "Lõppkuupäev")]
        public DateTime LoppKuupaev { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Hind { get; set; }

        [Required]
        [Range(1, 50)]
        public int MaxOsalejaid { get; set; }

        // Navigation property - initsialiseeritud
        public virtual ICollection<Registration> Registrations { get; set; }
            = new List<Registration>();
    }
}