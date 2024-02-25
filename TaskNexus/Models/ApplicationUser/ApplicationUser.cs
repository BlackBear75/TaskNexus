using Microsoft.AspNetCore.Identity;

namespace TaskNexus.Models.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        public string AvatarPhoto { get; set; }
    }
}
