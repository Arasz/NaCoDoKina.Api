using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Busies logic for disabling movies 
    /// </summary>
    public interface IDisabledMovieService
    {
        /// <summary>
        /// Disables movie for current user 
        /// </summary>
        /// <param name="movieId"> Deleted movie id </param>
        Task DisableMovieForCurrentUserAsync(long movieId);

        /// <summary>
        /// Returns only movies ids that are not disabled by current user 
        /// </summary>
        /// <param name="moviesIds"></param>
        /// <returns> Disabled movies ids </returns>
        Task<ICollection<long>> FilterDisabledMoviesForCurrentUserAsync(IEnumerable<long> moviesIds);

        /// <summary>
        /// Checks if user disabled given movie 
        /// </summary>
        /// <param name="movieId"> Movie disabled by user </param>
        /// <returns></returns>
        Task<bool> IsMovieDisabledForGivenUserAsync(long movieId);
    }
}