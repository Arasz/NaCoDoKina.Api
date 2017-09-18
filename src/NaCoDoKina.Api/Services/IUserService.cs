using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Returns current user id 
        /// </summary>
        /// <returns> Current user id </returns>
        Task<long> GetCurrentUserIdAsync();
    }
}