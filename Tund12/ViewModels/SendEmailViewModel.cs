using System.ComponentModel.DataAnnotations;

namespace Tund12.ViewModels
{
    public class SendEmailViewModel
    {
        public int TrainingId { get; set; }
        public string TrainingName { get; set; } = string.Empty;
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Teema on kohustuslik")]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sõnum on kohustuslik")]
        [StringLength(5000)]
        public string Body { get; set; } = string.Empty;
    }
}