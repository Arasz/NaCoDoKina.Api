using NaCoDoKina.Api.Models.Authentication;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Business logic for users 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets current user id 
        /// </summary>
        /// <returns> User id </returns>
        Task<long> GetCurrentUserIdAsync();

        /// <summary>
        /// Returns token for given user 
        /// </summary>
        /// <param name="userData"> Data necessary for uses authentication </param>
        /// <returns> User authentication token </returns>
        Task<AuthenticationToken> LoginAsync(UserData userData);

        /// <summary>
        /// Creates account for user 
        /// </summary>
        /// <param name="userData"> User data </param>
        Task RegisterAsync(UserData userData);
    }
}