using NaCoDoKina.Api.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Business logic contract for shows 
    /// </summary>
    public interface IShowService
    {
        /// <summary>
        /// Get all shows available near given location. 
        /// </summary>
        /// <param name="location"> User location </param>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        Task<IEnumerable<long>> GetAllShowsAsync(Location location);

        /// <summary>
        /// Get show basic information 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Basic show information </returns>
        Task<Show> GetShowAsync(long id);

        /// <summary>
        /// Mark show as deleted 
        /// </summary>
        /// <param name="id"> Show id </param>
        Task DeleteShowAsync(long id);

        /// <summary>
        /// Get detailed information about show 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Detailed information about show </returns>
        Task<ShowDetails> GetShowDetailsAsync(long id);
    }
}