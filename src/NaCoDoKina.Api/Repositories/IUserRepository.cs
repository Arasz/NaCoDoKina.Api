using NaCoDoKina.Api.Infrastructure.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Updates user password with new hashed password 
        /// </summary>
        /// <param name="user"> User </param>
        /// <param name="oldPassword"> Old password </param>
        /// <param name="newPassword"> New password </param>
        Task UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword);

        /// <summary>
        /// Increments failed access counter for user 
        /// </summary>
        /// <param name="user"> User </param>
        /// <returns></returns>
        Task IncrementAccessFailed(ApplicationUser user);

        /// <summary>
        /// Returns all registered users 
        /// </summary>
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        /// <summary>
        /// Returns user by name 
        /// </summary>
        /// <param name="userId"> User name </param>
        /// <returns> RegisterUser </returns>
        Task<ApplicationUser> GetUserByNameAsync(string userId);

        /// <summary>
        /// Returns user by name 
        /// </summary>
        /// <param name="email"> User email </param>
        /// <returns> RegisterUser </returns>
        Task<ApplicationUser> GetUserByEmailAsync(string email);

        /// <summary>
        /// Creates new user with password 
        /// </summary>
        /// <param name="user"> User </param>
        /// <param name="password"> Password </param>
        Task<bool> CreateUserWithPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns user by id 
        /// </summary>
        /// <param name="userId"> user id </param>
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        /// <summary>
        /// Returns user by external login provider 
        /// </summary>
        /// <param name="loginProvider"> Login provider name </param>
        /// <param name="key"> Login provider name </param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByLoginProviderAsync(string loginProvider, string key);
    }
}