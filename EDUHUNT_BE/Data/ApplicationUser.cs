using Microsoft.AspNetCore.Identity;

namespace EDUHUNT_BE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
