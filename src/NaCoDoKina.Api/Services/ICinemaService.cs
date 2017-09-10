using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public interface ICinemaService
    {
        /// <summary>
        /// Returns list of nearest cinemas which plays movie with given id 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="location"> User location </param>
        /// <returns> List of cinemas nearest to user location that play given movie </returns>
        Task<IEnumerable<Cinema>> GetNearestCinemasForMovie(long movieId, Location location);
    }
}