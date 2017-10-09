using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Results;
using Infrastructure.Models.Users;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Returns current user id 
        /// </summary>
        /// <returns> Current user id </returns>
        long GetCurrentUserId();

        /// <summary>
        /// Checks if user is registered and can be authenticated 
        /// </summary>
        /// <param name="user"> RegisterUser </param>
        /// <param name="password"> Password </param>
        /// <returns> True if successful authenticated </returns>
        Task<Result<User>> AuthenticateAsync(User user, string password);

        /// <summary>
        /// Returns all registered users 
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Returns user by name 
        /// </summary>
        /// <param name="userName"> RegisterUser name </param>
        /// <returns> RegisterUser </returns>
        Task<User> GetUserByNameAsync(string userName);

        /// <summary>
        /// Returns user by name 
        /// </summary>
        /// <param name="email"> RegisterUser email </param>
        /// <returns> RegisterUser </returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Creates new user with password 
        /// </summary>
        /// <param name="user"> RegisterUser </param>
        /// <param name="password"> Password </param>
        Task<Result<User>> CreateUserWithPasswordAsync(User user, string password);
    }
}