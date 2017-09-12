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
        Task<long> GetCurrentUserId();
    }
}