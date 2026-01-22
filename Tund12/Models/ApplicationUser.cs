using Microsoft.AspNetCore.Identity;

namespace Tund12.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}