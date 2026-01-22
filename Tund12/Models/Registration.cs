using System.ComponentModel.DataAnnotations;

namespace Tund12.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public int TrainingId { get; set; }
        public virtual Training? Training { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser? User { get; set; }

        [Required]
        [StringLength(20)]
        public string Staatus { get; set; } = "Ootel";

        public DateTime RegistreerimisAeg { get; set; } = DateTime.Now;
    }
}