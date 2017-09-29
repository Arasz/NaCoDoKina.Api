using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Models.Authentication;
using NaCoDoKina.Api.Models.Users;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Logic for creation of authentication token for user 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates authentication token for user 
        /// </summary>
        /// <param name="user"> RegisterUser for which token is generated </param>
        /// <returns> Authentication token </returns>
        AuthToken CreateToken(User user);
    }
}