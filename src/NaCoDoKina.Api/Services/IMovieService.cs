using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Business logic contract for shows 
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Get all shows available near given location. 
        /// </summary>
        /// <param name="location"> User location </param>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        Task<IEnumerable<long>> GetAllMoviesAsync(Location location);

        /// <summary>
        /// Get show basic information 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic show information </returns>
        Task<Movie> GetMovieAsync(long id);

        /// <summary>
        /// Mark show as deleted 
        /// </summary>
        /// <param name="id"> Movie id </param>
        Task DeleteMovieAsync(long id);

        /// <summary>
        /// Get detailed information about show 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Detailed information about show </returns>
        Task<MovieDetails> GetMovieDetailsAsync(long id);
    }
}