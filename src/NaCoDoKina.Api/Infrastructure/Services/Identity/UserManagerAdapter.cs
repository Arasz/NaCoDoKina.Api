using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Infrastructure.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    public class UserManagerAdapter : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerAdapter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return _userManager.Users
                .SingleOrDefaultAsync(user => user.UserName == username);
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }
    }
}