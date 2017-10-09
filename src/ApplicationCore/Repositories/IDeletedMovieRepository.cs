using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.Movies;

namespace ApplicationCore.Repositories
{
    /// <summary>
    /// Repository for deleted movies 
    /// </summary>
    public interface IDisabledMovieRepository
    {
        /// <summary>
        /// Creates new disabled movie 
        /// </summary>
        /// <param name="movieId"> Deleted movie id </param>
        /// <param name="userId"> User id </param>
        /// <returns></returns>
        Task<DisabledMovie> CreateDisabledMovieAsync(long movieId, long userId);

        /// <summary>
        /// Gets all disabled movies ids for user 
        /// </summary>
        /// <param name="userId"> User id </param>
        /// <returns> Disabled movies ids </returns>
        Task<IEnumerable<long>> GetDisabledMoviesIdsForUserAsync(long userId);

        /// <summary>
        /// Deletes disabled movie (DisabledMovie entity) 
        /// </summary>
        /// <param name="movieId"> Movie disabled by user </param>
        /// <param name="userId"> User which disabled movie </param>
        /// <returns></returns>
        Task<bool> DeleteDisabledMovieAsync(long movieId, long userId);

        /// <summary>
        /// Checks if user disabled given movie 
        /// </summary>
        /// <param name="movieId"> Movie disabled by user </param>
        /// <param name="userId"> User which disabled movie </param>
        /// <returns></returns>
        Task<bool> IsMovieDisabledAsync(long movieId, long userId);
    }
}