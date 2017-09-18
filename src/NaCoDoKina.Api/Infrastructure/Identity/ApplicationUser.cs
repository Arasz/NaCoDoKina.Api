using Microsoft.AspNetCore.Identity;

namespace NaCoDoKina.Api.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool CanChangePassword { get; set; }
    }
}