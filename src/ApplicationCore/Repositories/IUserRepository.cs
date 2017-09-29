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
        /// <returns> True if succeeded </returns>
        Task<bool> UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword);

        /// <summary>
        /// Increments failed access counter for user 
        /// </summary>
        /// <param name="user"> User </param>
        /// <returns> True if succeeded </returns>
        Task<bool> IncrementAccessFailed(ApplicationUser user);

        /// <summary>
        /// Returns all registered users 
        /// </summary>
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        /// <summary>
        /// Returns user by name 
        /// </summary>
        /// <returns> RegisterUser </returns>
        Task<ApplicationUser> GetUserByNameAsync(string userName);

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
        Task<ApplicationUser> CreateUserWithPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns user by id 
        /// </summary>
        /// <param name="userId"> user id </param>
        Task<ApplicationUser> GetUserByIdAsync(long userId);
    }
}