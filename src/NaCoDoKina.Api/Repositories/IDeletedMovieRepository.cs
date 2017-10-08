using NaCoDoKina.Api.Entities.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
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
        Task<IEnumerable<long>> GetDisabledMoviesIdsForUser(long userId);

        /// <summary>
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteDisabledMovieAsync(long movieId, long userId);
    }
}