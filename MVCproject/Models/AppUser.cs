using Microsoft.AspNetCore.Identity;

namespace MVCproject.Models
{
    public class AppUser : IdentityUser
    {
        public bool UserStatus { get; set; } = false;
        public DateTime CreateAt { get; set; }
    }
}
