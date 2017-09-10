using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Bussiness logic for cinemas 
    /// </summary>
    public interface ICinemaService
    {
        /// <summary>
        /// Returns list of nearest cinemas which plays movie with given id 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="location"> User location </param>
        /// <returns> List of cinemas nearest to user location that play given movie </returns>
        Task<IEnumerable<Cinema>> GetNearestCinemasForMovieAsync(long movieId, Location location);

        /// <summary>
        /// Returns list of nearest cinemas 
        /// </summary>
        /// <param name="location"> User location </param>
        /// <returns> List of nearest cinemas </returns>
        Task<IEnumerable<Cinema>> GetNearestCinemasAsync(Location location);

        /// <summary>
        /// Adds new cinema 
        /// </summary>
        /// <param name="cinema"></param>
        /// <returns></returns>
        Task<Cinema> AddCinemaAsync(Cinema cinema);
    }
}