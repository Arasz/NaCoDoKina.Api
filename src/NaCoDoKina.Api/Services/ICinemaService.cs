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
        /// <param name="searchArea"> User searchArea </param>
        /// <returns> List of cinemas nearest to user searchArea that play given movie </returns>
        Task<IEnumerable<Cinema>> GetNearestCinemasForMovieAsync(long movieId, SearchArea searchArea);

        /// <summary>
        /// Returns list of nearest cinemas 
        /// </summary>
        /// <param name="searchArea"> User searchArea </param>
        /// <returns> List of nearest cinemas </returns>
        Task<IEnumerable<Cinema>> GetNearestCinemasAsync(SearchArea searchArea);

        /// <summary>
        /// Adds new cinema 
        /// </summary>
        /// <param name="cinema"></param>
        /// <returns></returns>
        Task<Cinema> AddCinemaAsync(Cinema cinema);
    }
}