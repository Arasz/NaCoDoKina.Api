using Microsoft.AspNetCore.Identity;
using NaCoDoKina.Api.Infrastructure.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    /// <summary>
    /// Adapter service for user manager 
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Create user in backing store 
        /// </summary>
        /// <param name="user"> Created user </param>
        /// <param name="password"> User password </param>
        /// <returns> Creation result </returns>
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns id of user represented as principal object 
        /// </summary>
        /// <param name="principal"> User representation </param>
        /// <returns> User id </returns>
        string GetUserId(ClaimsPrincipal principal);
    }
}