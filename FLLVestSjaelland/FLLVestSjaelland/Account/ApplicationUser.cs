using Microsoft.AspNetCore.Identity;

namespace FLLVestSjaelland.Account
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
