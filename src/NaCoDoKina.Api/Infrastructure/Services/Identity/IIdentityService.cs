using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Token;
using NaCoDoKina.Api.Results;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    /// <summary>
    /// User identity logic 
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// LoginAsync user and return authentication token 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Result<AuthenticationToken>> LoginAsync(string userName, string password);

        /// <summary>
        /// Creates new user 
        /// </summary>
        /// <param name="user"> User data </param>
        /// <param name="password"> User password </param>
        /// <returns></returns>
        Task<Result> CreateUserAsync(ApplicationUser user, string password);

        /// <summary>
        /// Verifies user password 
        /// </summary>
        /// <param name="user"> User data </param>
        /// <param name="password"> User password </param>
        /// <returns></returns>
        bool VerifyPassword(ApplicationUser user, string password);

        /// <summary>
        /// Returns user with given name 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByNameAsync(string username);

        /// <summary>
        /// Returns current user id 
        /// </summary>
        /// <returns></returns>
        long GetCurrentUserId();
    }
}