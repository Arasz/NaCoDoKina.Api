using Microsoft.AspNetCore.Identity;
using NaCoDoKina.Api.Infrastructure.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    /// <summary>
    /// Adapter service for sign in manager 
    /// </summary>
    public interface ISignInManager
    {
        /// <summary>
        /// Signs in user with password 
        /// </summary>
        /// <param name="username"> Username </param>
        /// <param name="password"> Password </param>
        /// <returns> Result </returns>
        Task<SignInResult> PasswordSignInAsync(string username, string password);

        /// <summary>
        /// Signs in specified usr 
        /// </summary>
        /// <param name="user"> Signed in user </param>
        /// <returns></returns>
        Task SignInAsync(ApplicationUser user);

        /// <summary>
        /// User for current http context 
        /// </summary>
        ClaimsPrincipal CurrentContextUser { get; }
    }
}