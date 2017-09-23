using NaCoDoKina.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Movie showtime business logic 
    /// </summary>
    public interface IMovieShowtimeService
    {
        /// <summary>
        /// Returns all show times for movie 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="laterThan"></param>
        /// <param name="laterThan"> Minimum movie show time date time </param>
        /// <returns> List of movie show times played in all cinemas </returns>
        Task<IEnumerable<MovieShowtime>> GetMovieShowtimesAsync(long movieId, DateTime laterThan);

        /// <summary>
        /// Returns all show times for movie in given cinema 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="cinemaId"> Cinema id </param>
        /// <param name="laterThan"> Minimum movie show time date time </param>
        /// <returns> List of movie show times played in given cinema </returns>
        Task<IEnumerable<MovieShowtime>> GetMovieShowtimesForCinemaAsync(long movieId, long cinemaId, DateTime laterThan);
    }
}